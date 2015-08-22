#ifndef _Singleton_h_
#define _Singleton_h_

template<typename T>
class SingletonFactory
{
	public:
	static T* instance()
	{
		return new T();
	}
};
template<typename T, typename MANA = SingletonFactory<T> >
class Singleton
{
	private:
		Singleton(const Singleton&);
		const Singleton & operator= (const Singleton &);
	protected:
		static T* ms_Singleton;
		Singleton(void)
		{}
		~Singleton(void)
		{}
	public:
		static T* instance(void)
		{
			if(!ms_Singleton)
			{
				ms_Singleton = MANA::instance();
			}
			return ms_Singleton;
		}
		static T& getMe()
		{
			return *instance();
		}
		static void delMe(void)
		{
			if(ms_Singleton)
			{
				delete ms_Singleton;
				ms_Singleton = 0;
			}
		}
};
template<typename T, typename MANA>
T* Singleton<T, MANA>::ms_Singleton = 0;

#endif

