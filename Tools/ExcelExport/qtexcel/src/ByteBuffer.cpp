#include "ByteBuffer.hxx"
#include "Tools.hxx"

void ByteBufferException::PrintPosError() const
{
	
}

void ByteBuffer::print_storage() const
{
    std::ostringstream ss;
    ss <<  "STORAGE_SIZE: " << size() << "\n";
    ss << "         ";

    for (size_t i = 0; i < size(); ++i)
        ss << uint32(read<uint8>(i)) << " - ";

	Tools::getSingletonPtr()->Log(Tools::getSingletonPtr()->GBKChar2UNICODEStr(ss.str().c_str()));
}

void ByteBuffer::textlike() const
{
    std::ostringstream ss;
    ss <<  "STORAGE_SIZE: " << size() << "\n";

    ss << "         ";

    for (size_t i = 0; i < size(); ++i)
        ss << read<uint8>(i);

	Tools::getSingletonPtr()->Log(Tools::getSingletonPtr()->GBKChar2UNICODEStr(ss.str().c_str()));
}

void ByteBuffer::hexlike() const
{
    std::ostringstream ss;
    ss <<  "STORAGE_SIZE: " << size() << "\n";

    ss << "         ";

    size_t j = 1, k = 1;

    for (size_t i = 0; i < size(); ++i)
    {
        if ((i == (j * 8)) && ((i != (k * 16))))
        {
            ss << "| ";
            ++j;
        }
        else if (i == (k * 16))
        {
            ss << "\n";

            ss << "         ";

            ++k;
            ++j;
        }

        char buf[4];
        _snprintf(buf, 4, "%02X", read<uint8>(i));
        ss << buf << " ";
    }

	Tools::getSingletonPtr()->Log(Tools::getSingletonPtr()->GBKChar2UNICODEStr(ss.str().c_str()));
}

void ByteBuffer::writeFile(FILE* file)
{
	fwrite((void*)&_storage[0], sizeof(char), this->size(), file);
}