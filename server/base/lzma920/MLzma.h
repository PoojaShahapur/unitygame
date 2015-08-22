#ifndef __MLzma_H
#define __MLzma_H


#define LZMA_HEADER_LEN 13

/**
 * @brief ִ�� LZMA ѹ��
 */
class MLzma
{
public:
	MLzma();
	~MLzma();

	static bool LzmaFileCompress(const char* scrfilename, const char* desfilename);			// �ļ�ѹ��
	static bool LzmaFileUncompress(const char* scrfilename, const char* desfilename);		// �ļ���ѹ��

	static bool LzmaStrCompress(const char* scrStr, unsigned int srcLen, unsigned char* desStr, unsigned int* destLen = NULL);					// �ַ���ѹ��
	static bool LzmaStrUncompress(const unsigned char* scrStr, unsigned int srcLen, unsigned char* desStr, unsigned int* destLen = NULL);				// �ַ�����ѹ��
};

#endif

