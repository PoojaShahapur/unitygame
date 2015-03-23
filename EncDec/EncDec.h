#ifndef _ENCDEC_H_
#define _ENCDEC_H_

#ifndef _MY_RC5_H
#define _MY_RC5_H
#include <stdlib.h>

#define RC5_ENCRYPT	1
#define RC5_DECRYPT	0

/* 32 bit.  For Alpha,things may get weird */
#define RC5_32_INT unsigned int

#define RC5_32_BLOCK		8
#define RC5_32_KEY_LENGTH	16 /* This is a default,max is 255 */

/* This are the only values supported.  Tweak the code if you want more
 * * The most supported modes will be
 * * RC5-32/12/16
 * * RC5-32/16/8
 * */
#define RC5_8_ROUNDS	8
#define RC5_12_ROUNDS	12
#define RC5_16_ROUNDS	16

typedef struct rc5_key_st
{
	/* Number of rounds */
	int rounds;
	RC5_32_INT data[2*(RC5_16_ROUNDS+1)];
} RC5_32_KEY;

#define c2ln(c,l1,l2,n)	{ \
	c+=n; \
		l1=l2=0; \
		switch (n) { \
			case 8: l2 =((RC5_32_INT )(*(--(c))))<<24L; \
			case 7: l2|=((RC5_32_INT )(*(--(c))))<<16L; \
			case 6: l2|=((RC5_32_INT )(*(--(c))))<< 8L; \
			case 5: l2|=((RC5_32_INT )(*(--(c))));     \
			case 4: l1 =((RC5_32_INT )(*(--(c))))<<24L; \
			case 3: l1|=((RC5_32_INT )(*(--(c))))<<16L; \
			case 2: l1|=((RC5_32_INT )(*(--(c))))<< 8L; \
			case 1: l1|=((RC5_32_INT )(*(--(c))));     \
		} \
}

#undef c2l
#define c2l(c,l)	(l =((unsigned int)(*((c)++)))    ,\
		l|=((unsigned int)(*((c)++)))<< 8L,\
		l|=((unsigned int)(*((c)++)))<<16L,\
		l|=((unsigned int)(*((c)++)))<<24L)
		
#define RC5_32_MASK	0xffffffffL

#define RC5_16_P	0xB7E1
#define RC5_16_Q	0x9E37
#define RC5_32_P	0xB7E15163L
#define RC5_32_Q	0x9E3779B9L
#define RC5_64_P	0xB7E151628AED2A6BLL
#define RC5_64_Q	0x9E3779B97F4A7C15LL

#define ROTATE_l32(a,n)     (((a)<<(int)(n))|((a)>>(32-(int)(n))))
#define ROTATE_r32(a,n)     (((a)>>(int)(n))|((a)<<(32-(int)(n))))
	/*
	 * #define ROTATE_l32(a,n)     _lrotl(a,n)
	 * #define ROTATE_r32(a,n)     _lrotr(a,n)
	 * */

#define E_RC5_32(a,b,s,n) \
	a^=b; \
	a=ROTATE_l32(a,b); \
	a+=s[n]; \
	a&=RC5_32_MASK; \
	b^=a; \
	b=ROTATE_l32(b,a); \
	b+=s[n+1]; \
	b&=RC5_32_MASK;

#define D_RC5_32(a,b,s,n) \
b-=s[n+1]; \
	b&=RC5_32_MASK; \
	b=ROTATE_r32(b,a); \
	b^=a; \
	a-=s[n]; \
	a&=RC5_32_MASK; \
	a=ROTATE_r32(a,b); \
	a^=b;

	
	//这里需要添加代码 88-92行
#endif
	
#ifndef _MY_DES_H
#define _MY_DES_H
#include <stdlib.h>
#define  DES_ENCRYPT	1
#define  DES_DECRYPT	0

//#define  DES_LONG unsigned long
#define DES_LONG unsigned int
typedef unsigned char  DES_cblock[8];
typedef /* const */ unsigned char const_DES_cblock[8];
typedef DES_LONG t_DES_SPtrans[8][64];

extern t_DES_SPtrans MyDES_SPtrans;
/* With "const",gcc 2.8.1 on Solaris thinks that  DES_cblock *
 * * and const_ DES_cblock * are incompatible pointer types. */

typedef struct  DES_ks
{
	union
	{
		 DES_cblock cblock;
		/* make sure things are correct size on machines with
		 * 		* 8 unsigned char longs */
		 DES_LONG deslong[2];
	} ks[16];
}  DES_key_schedule;

extern const DES_LONG (*sp)[8][64];

#define	ROTATE(a,n)	(((a)>>(int)(n))|((a)<<(32-(int)(n))))
#define  DES_KEY_SZ 	(sizeof( DES_cblock))

#define LOAD_DATA(R,S,u,t,E0,E1,tmp) \
u=R^s[S  ]; \
	t=R^s[S+1]
#define LOAD_DATA_tmp(a,b,c,d,e,f) LOAD_DATA(a,b,c,d,e,f,g)

#define D_ENCRYPT(LL,R,S) {\
	LOAD_DATA_tmp(R,S,u,t,E0,E1); \
		t=ROTATE(t,4); \
		LL^=\
		 DES_SPtrans[0][(u>> 2L)&0x3f]^ \
		 DES_SPtrans[2][(u>>10L)&0x3f]^ \
		 DES_SPtrans[4][(u>>18L)&0x3f]^ \
		 DES_SPtrans[6][(u>>26L)&0x3f]^ \
		 DES_SPtrans[1][(t>> 2L)&0x3f]^ \
		 DES_SPtrans[3][(t>>10L)&0x3f]^ \
		 DES_SPtrans[5][(t>>18L)&0x3f]^ \
		 DES_SPtrans[7][(t>>26L)&0x3f]; }

#define PERM_OP(a,b,t,n,m) ((t)=((((a)>>(n))^(b))&(m)),\
		(b)^=(t),\
		(a)^=((t)<<(n)))

#define IP(l,r) \
{ \
	register  DES_LONG tt; \
		PERM_OP(r,l,tt,4,0x0f0f0f0fL); \
		PERM_OP(l,r,tt,16,0x0000ffffL); \
		PERM_OP(r,l,tt,2,0x33333333L); \
		PERM_OP(l,r,tt,8,0x00ff00ffL); \
		PERM_OP(r,l,tt,1,0x55555555L); \
}

#define FP(l,r) \
{ \
	register  DES_LONG tt; \
		PERM_OP(l,r,tt,1,0x55555555L); \
		PERM_OP(r,l,tt,8,0x00ff00ffL); \
		PERM_OP(l,r,tt,2,0x33333333L); \
		PERM_OP(r,l,tt,16,0x0000ffffL); \
		PERM_OP(l,r,tt,4,0x0f0f0f0fL); \
}

#define HPERM_OP(a,t,n,m) ((t)=((((a)<<(16-(n)))^(a))&(m)),\
		(a)=(a)^(t)^(t>>(16-(n))))
#define ITERATIONS 16
#define HALF_ITERATIONS 8

#define MAXWRITE (1024*16)
#define BSIZE (MAXWRITE+4)
#undef c2l
#define c2l(c,l)	(l =((unsigned int)(*((c)++)))    ,\
		l|=((unsigned int)(*((c)++)))<< 8L,\
		l|=((unsigned int)(*((c)++)))<<16L,\
		l|=((unsigned int)(*((c)++)))<<24L)		
		
		
	//这里需要代码 185-197行
#endif	
		
	//缺少cast 缺少 idea
		
extern void  DES_random_key( DES_cblock *ret);
extern void  DES_set_key(const_DES_cblock *key, DES_key_schedule *schedule);
extern void  DES_encrypt1( DES_LONG *data, DES_key_schedule *ks,int enc);
extern void  DES_encrypt3( DES_LONG *data, DES_key_schedule *ks1, DES_key_schedule *ks2, DES_key_schedule *ks3);
extern void  DES_decrypt3( DES_LONG *data, DES_key_schedule *ks1, DES_key_schedule *ks2, DES_key_schedule *ks3);

extern void RC5_32_set_key(RC5_32_KEY *key,int len,const unsigned char *data,int rounds);
extern void RC5_32_encrypt(RC5_32_INT *d,RC5_32_KEY *key);
extern void RC5_32_decrypt(RC5_32_INT *d,RC5_32_KEY *key);

class CEncrypt
{
public:
  CEncrypt();
  enum encMethod
  {
    ENCDEC_NONE,
    ENCDEC_DES,
    ENCDEC_RC5
  };
  void random_key_des(DES_cblock *ret);
  void set_key_des(const_DES_cblock *key);
  void set_key_rc5(const unsigned char *data,int nLen,int rounds);
  int encdec(void *data,unsigned int nLen,bool enc);

  void setEncMethod(encMethod method);
  encMethod getEncMethod() const;
  void setEncMask(unsigned int m);
  void setDecMask(unsigned int m);
private:
  void DES_random_key(DES_cblock *ret);
  void DES_set_key(const_DES_cblock *key,DES_key_schedule *schedule);
  void DES_encrypt1(DES_LONG *data,DES_key_schedule *ks,int enc);

  void RC5_32_set_key(RC5_32_KEY *key,int len,const unsigned char *data,int rounds);
  void RC5_32_encrypt(RC5_32_INT *d,RC5_32_KEY *key);
  void RC5_32_decrypt(RC5_32_INT *d,RC5_32_KEY *key);

  int encdec_des(unsigned char *data,unsigned int nLen,bool enc);
  int encdec_rc5(unsigned char *data,unsigned int nLen,bool enc);

  DES_key_schedule key_des;
  RC5_32_KEY key_rc5;
  bool haveKey_des;
  bool haveKey_rc5;

  encMethod method;
    
  unsigned int enc_mask;
  unsigned int dec_mask;
};

#endif
