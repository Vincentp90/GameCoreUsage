#include <boost/interprocess/shared_memory_object.hpp>

#pragma warning(disable : 4996)

extern "C" __declspec(dllexport) const void __cdecl test(char* filename, char* buf, int buflength) {
    std::string shmfile;
    boost::interprocess::ipcdetail::create_shared_dir_cleaning_old_and_get_filepath(filename, shmfile);
    strcpy_s(buf, buflength, shmfile.c_str());
}
