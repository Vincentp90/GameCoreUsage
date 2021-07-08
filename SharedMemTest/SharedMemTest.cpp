#include <boost/interprocess/shared_memory_object.hpp>
#include <boost/interprocess/windows_shared_memory.hpp>
#include <boost/interprocess/mapped_region.hpp>
#include <cstring>
#include <cstdlib>
#include <string>

#include <iostream>

int main(int argc, char* argv[])
{
    using namespace boost::interprocess;

    const int FRAMETIMES_INT_SIZE = 5000;

    //https://www.boost.org/doc/libs/1_54_0/doc/html/interprocess/sharedmemorybetweenprocesses.html

    if (argc == 1) {  //Parent process
       //Remove shared memory on construction and destruction
        struct shm_frametimes_remove
        {
            shm_frametimes_remove() { shared_memory_object::remove("SHMFRAMETIMES"); }
            ~shm_frametimes_remove() { shared_memory_object::remove("SHMFRAMETIMES"); }
        } frametimes_remover;
        struct shm_index_remove
        {
            shm_index_remove() { shared_memory_object::remove("SHMINDEX"); }
            ~shm_index_remove() { shared_memory_object::remove("SHMINDEX"); }
        } index_remover;

        //Create a shared memory object.
        shared_memory_object shmframetimes(create_only, "SHMFRAMETIMES", read_write);
        shared_memory_object shmindex(create_only, "SHMINDEX", read_write);

        //Set size
        shmframetimes.truncate(FRAMETIMES_INT_SIZE*sizeof(int));
        shmindex.truncate(sizeof(int));

        //Map the whole shared memory in this process
        mapped_region regionFrametimes(shmframetimes, read_write);
        mapped_region regionIndex(shmindex, read_write);

        int* index = (int*)regionIndex.get_address();
        while (true)
        {
            int* frametime = static_cast<int*>(regionFrametimes.get_address());
            for (int i = 0; i < FRAMETIMES_INT_SIZE;i++)
            {
                *frametime = 50 + rand() % 50;
                //std::cout << "time: " << *frametime << " ";
                boost::interprocess::winapi::sleep(*frametime);
                frametime++;
                *index = i;
            }
            std::cout << "completed loop" << std::endl;
        }
    }
    else if(std::string(argv[1]) == "win") {
        windows_shared_memory shmframetimes(open_or_create, "SHMFRAMETIMES", read_write, FRAMETIMES_INT_SIZE * sizeof(int));
        windows_shared_memory shmindex(open_or_create, "SHMINDEX", read_write, sizeof(int));

        mapped_region regionFrametimes(shmframetimes, read_write);
        mapped_region regionIndex(shmindex, read_write);

        int* index = (int*)regionIndex.get_address();
        while (true)
        {
            int* frametime = static_cast<int*>(regionFrametimes.get_address());
            for (int i = 0; i < FRAMETIMES_INT_SIZE; i++)
            {
                *frametime = 50 + rand() % 50;
                //std::cout << "time: " << *frametime << " ";
                boost::interprocess::winapi::sleep(*frametime);
                frametime++;
                *index = i;
            }
            std::cout << "completed loop" << std::endl;
        }
    }
    else {
        std::cout << "Starting shared mem read" << std::endl;
        std::cout << argv[1] << std::endl;

        //Open already created shared memory object.
        shared_memory_object shmframetimes(open_only, "SHMFRAMETIMES", read_only);
        shared_memory_object shmindex(open_only, "SHMINDEX", read_only);

        std::cout << "mapping region..." << std::endl;
        //Map the whole shared memory in this process
        mapped_region regionFrametimes(shmframetimes, read_only);
        mapped_region regionIndex(shmindex, read_only);

        std::cout << "read loop..." << std::endl;
        int ownIndex = 0;
        for (int i = 0; i < 1500; i++) {        
            int* index = static_cast<int*>(regionIndex.get_address());
            int* frametime = static_cast<int*>(regionFrametimes.get_address());
            frametime = frametime + ownIndex;

            std::cout << "ownIndex: " << ownIndex << std::endl;
            std::cout << "Index: " << *index << std::endl;

            int total = 0;
            int count = 0;
            //Test Wrap around
            for (int j = ownIndex; j % FRAMETIMES_INT_SIZE != *index; j++)
            {
                //std::cout << "j: " << j % FRAMETIMES_INT_SIZE << std::endl;
                if (j % FRAMETIMES_INT_SIZE == 0)
                {
                    frametime = static_cast<int*>(regionIndex.get_address());
                }
                else
                {
                    frametime++;
                }
                //std::cout << " " << *frametime;
                count++;                
                total += *frametime;
            }
            //std::cout << std::endl << "total: " << total << std::endl;
            //std::cout << "count: " << count << std::endl;
            if (count > 0) {
                double avg = ((double)total / (double)count) / 1000.00;
                std::cout << "Calculated avg frametime: " << avg << std::endl;
            }            
            ownIndex = *index;
            boost::interprocess::winapi::sleep(200);
        }

        //Check that memory was initialized to 1
        //int* mem = static_cast<int*>(region.get_address());
        //for (std::size_t i = 0; i < region.get_size(); ++i)
        //    std::cout << *mem++;
        //    //if (*mem++ != 1)
        //    return 1;   //Error checking memory
    }
    return 0;
}