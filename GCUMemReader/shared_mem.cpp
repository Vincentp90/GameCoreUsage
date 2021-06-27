#include <boost/interprocess/shared_memory_object.hpp>

extern "C"             //No name mangling
__declspec(dllexport)  //Tells the compiler to export the function
std::string             //Function return type     
__cdecl                //Specifies calling convention, cdelc is default, 
                       //so this can be omitted 

    test(char* filename) {
    std::string shmfile;
    boost::interprocess::ipcdetail::create_shared_dir_cleaning_old_and_get_filepath(filename, shmfile);
    return shmfile;
}
