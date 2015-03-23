#include "EncDec.h"
#include <string.h>

CEncrypt::CEncrypt()
{
  memset(&key_des, 0, sizeof(key_des));
  memset(&key_rc5, 0, sizeof(key_rc5));
  haveKey_des = false;
  haveKey_rc5 = false;
  method = ENCDEC_NONE;
  enc_mask = dec_mask = 0;
}

void CEncrypt::DES_set_key(const_DES_cblock *key, DES_key_schedule *schedule)
{
  ::DES_set_key(key,schedule);
}

void CEncrypt::DES_random_key( DES_cblock *ret)
{
  ::DES_random_key(ret);
}

void CEncrypt::DES_encrypt1( DES_LONG *data, DES_key_schedule *ks,int enc)
{
  ::DES_encrypt1(data,ks,enc);
}

void CEncrypt::RC5_32_set_key(RC5_32_KEY *key,int len,const unsigned char *data,int rounds)
{
  ::RC5_32_set_key(key,len,data,rounds);
}

void CEncrypt::RC5_32_encrypt(RC5_32_INT*d,RC5_32_KEY *key)
{
  ::RC5_32_encrypt(d,key);
}

void CEncrypt::RC5_32_decrypt(RC5_32_INT*d,RC5_32_KEY *key)
{
  ::RC5_32_decrypt(d,key);
}
#define ROTATE_LEFT(x, n) (((x) << (n)) | ((x) >> (32-(n))))
int CEncrypt::encdec_des(unsigned char *data,unsigned int nLen,bool enc)
{
    if ((0==data)||(!haveKey_des)) return -1;

    unsigned int offset = 0;
    while (offset<=nLen-8)
    {      
	if(0x80000000 & (enc?enc_mask:dec_mask))
	    DES_encrypt1(( DES_LONG*)(data+offset),&key_des,enc);
	offset += 8;
	if(enc)
	    enc_mask = ROTATE_LEFT(enc_mask, 1);
	else
	    dec_mask = ROTATE_LEFT(dec_mask, 1);
    }

    return nLen-offset;
}

int CEncrypt::encdec_rc5(unsigned char *data,unsigned int nLen,bool enc)
{
    if ((0==data)||(!haveKey_rc5)) return -1;

    unsigned int offset = 0;
    while (offset<=nLen-8)
    {
	RC5_32_INT d[2];
	if(0x80000000 & (enc?enc_mask:dec_mask))
	{
	    memcpy(d,data+offset,sizeof(d));
	    if (enc)
		::RC5_32_encrypt(d,&key_rc5);
	    else
		::RC5_32_decrypt(d,&key_rc5);
	    memcpy(data+offset,d,sizeof(d));
	}
	offset += sizeof(d);
	if(enc)
	    enc_mask = ROTATE_LEFT(enc_mask, 1);
	else
	    dec_mask = ROTATE_LEFT(dec_mask, 1);
    }

    return nLen-offset;
}

void CEncrypt::random_key_des( DES_cblock *ret)
{
  ::DES_random_key(ret);
}

void CEncrypt::set_key_des(const_DES_cblock *key)
{
  ::DES_set_key(key,&key_des);
  haveKey_des = true;
}

void CEncrypt::set_key_rc5(const unsigned char *data,int nLen,int rounds)
{
  ::RC5_32_set_key(&key_rc5,nLen,data,rounds);
  haveKey_rc5 = true;
} 

int CEncrypt::encdec(void *data,unsigned int nLen,bool enc)
{
  switch(method)
  {
    case ENCDEC_NONE:
         return -1;
    case ENCDEC_DES:
         return encdec_des((unsigned char*)data,nLen,enc);
    case ENCDEC_RC5:
         return encdec_rc5((unsigned char*)data,nLen,enc);
  }
  return -2;
}

void CEncrypt::setEncMethod(encMethod m)
{
  method = m;
  enc_mask = dec_mask = 0xffffffff;
}

CEncrypt::encMethod CEncrypt::getEncMethod() const
{
  return method;
}

void CEncrypt::setEncMask(unsigned int m)
{
    enc_mask = m;
}

void CEncrypt::setDecMask(unsigned int m)
{
    dec_mask = m;
}
