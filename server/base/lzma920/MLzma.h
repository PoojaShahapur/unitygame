#ifndef __MLzma_H
#define __MLzma_H


#define LZMA_HEADER_LEN 13

/**
 * @brief Ö´ĞĞ LZMA Ñ¹Ëõ
 */
class MLzma
{
public:
	MLzma();
	~MLzma();

	static bool LzmaFileCompress(const char* scrfilename, const char* desfilename);			// ÎÄ¼şÑ¹Ëõ
	static bool LzmaFileUncompress(const char* scrfilename, const char* desfilename);		// ÎÄ¼ş½âÑ¹Ëõ

	static bool LzmaStrCompress(const char* scrStr, unsigned int srcLen, unsigned char* desStr, unsigned int* destLen = NULL);					// ×Ö·û´®Ñ¹Ëõ
	static bool LzmaStrUncompress(const unsigned char* scrStr, unsigned int srcLen, unsigned char* desStr, unsigned int* destLen = NULL);				// ×Ö·û´®½âÑ¹Ëõ
};

#endif

