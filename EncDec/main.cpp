//#include "mydes.h"

#include "EncDec.h"

void main()
{
	//extern void DES_set_key(const_DES_cblock *key, DES_key_schedule *schedule);
	//extern void DES_encrypt1(DES_LONG *data, DES_key_schedule *ks1, int enc);

	const_DES_cblock key[8] = { 0x65, 0xC1, 0x78, 0xB2, 0x84, 0xD1, 0x97, 0xCC };
	DES_key_schedule schedule;

	unsigned char data[8] = { 0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1 };

	//DES_set_key(key, &schedule);
	//DES_encrypt1(data, &schedule, 0);

	CEncrypt crypt;
	crypt.set_key_des(key);
	crypt.setEncMethod(CEncrypt::ENCDEC_DES);
	crypt.encdec(data, 8, 1);
}