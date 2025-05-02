#include "pch-cpp.hpp"






struct IntPtrU5BU5D_tFD177F8C806A6921AD7150264CCC62FA00CAD832;
struct StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF;
struct ArgumentOutOfRangeException_tEA2822DAF62B10EEED00E0E3A341D4BAF78CF85F;
struct IDictionary_t6D03155AF1FA9083817AA5B6AD7DEEACC26AB220;
struct SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6;
struct String_t;
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915;

IL2CPP_EXTERN_C RuntimeClass* ArgumentOutOfRangeException_tEA2822DAF62B10EEED00E0E3A341D4BAF78CF85F_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C String_t* _stringLiteralCF088CD7CED04705BD5E7559DBB7DD9A14559182;
IL2CPP_EXTERN_C const RuntimeMethod* TimeoutTracker__ctor_mBFE73B99C5BBB93D251DF9731B9C5D824F30CDCE_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* TimeoutTracker_get_IsExpired_m5E08DCC01117EF66ADEBA5472B27833577289813_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* TimeoutTracker_get_RemainingMilliseconds_mDAA8A0E545F8E28D3688095CA346B0A04337BC23_RuntimeMethod_var;
struct Exception_t_marshaled_com;
struct Exception_t_marshaled_pinvoke;


IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct String_t  : public RuntimeObject
{
	int32_t ____stringLength;
	Il2CppChar ____firstChar;
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22 
{
	bool ___m_value;
};
struct Int32_t680FF22E76F6EFAD4375103CBBFFA0421349384C 
{
	int32_t ___m_value;
};
struct IntPtr_t 
{
	void* ___m_value;
};
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};
struct TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C 
{
	int32_t ___m_total;
	int32_t ___m_start;
};
struct Exception_t  : public RuntimeObject
{
	String_t* ____className;
	String_t* ____message;
	RuntimeObject* ____data;
	Exception_t* ____innerException;
	String_t* ____helpURL;
	RuntimeObject* ____stackTrace;
	String_t* ____stackTraceString;
	String_t* ____remoteStackTraceString;
	int32_t ____remoteStackIndex;
	RuntimeObject* ____dynamicMethods;
	int32_t ____HResult;
	String_t* ____source;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces;
	IntPtrU5BU5D_tFD177F8C806A6921AD7150264CCC62FA00CAD832* ___native_trace_ips;
	int32_t ___caught_in_unmanaged;
};
struct Exception_t_marshaled_pinvoke
{
	char* ____className;
	char* ____message;
	RuntimeObject* ____data;
	Exception_t_marshaled_pinvoke* ____innerException;
	char* ____helpURL;
	Il2CppIUnknown* ____stackTrace;
	char* ____stackTraceString;
	char* ____remoteStackTraceString;
	int32_t ____remoteStackIndex;
	Il2CppIUnknown* ____dynamicMethods;
	int32_t ____HResult;
	char* ____source;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces;
	Il2CppSafeArray* ___native_trace_ips;
	int32_t ___caught_in_unmanaged;
};
struct Exception_t_marshaled_com
{
	Il2CppChar* ____className;
	Il2CppChar* ____message;
	RuntimeObject* ____data;
	Exception_t_marshaled_com* ____innerException;
	Il2CppChar* ____helpURL;
	Il2CppIUnknown* ____stackTrace;
	Il2CppChar* ____stackTraceString;
	Il2CppChar* ____remoteStackTraceString;
	int32_t ____remoteStackIndex;
	Il2CppIUnknown* ____dynamicMethods;
	int32_t ____HResult;
	Il2CppChar* ____source;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces;
	Il2CppSafeArray* ___native_trace_ips;
	int32_t ___caught_in_unmanaged;
};
struct SystemException_tCC48D868298F4C0705279823E34B00F4FBDB7295  : public Exception_t
{
};
struct ArgumentException_tAD90411542A20A9C72D5CDA3A84181D8B947A263  : public SystemException_tCC48D868298F4C0705279823E34B00F4FBDB7295
{
	String_t* ____paramName;
};
struct ArgumentOutOfRangeException_tEA2822DAF62B10EEED00E0E3A341D4BAF78CF85F  : public ArgumentException_tAD90411542A20A9C72D5CDA3A84181D8B947A263
{
	RuntimeObject* ____actualValue;
};
struct String_t_StaticFields
{
	String_t* ___Empty;
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	String_t* ___TrueString;
	String_t* ___FalseString;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif



IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ArgumentOutOfRangeException__ctor_mBC1D5DEEA1BA41DE77228CB27D6BAFEB6DCCBF4A (ArgumentOutOfRangeException_tEA2822DAF62B10EEED00E0E3A341D4BAF78CF85F* __this, String_t* ___0_paramName, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Environment_get_TickCount_m183BFCF001E12849E3D898957F0B81FD88BA183C (const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void TimeoutTracker__ctor_mBFE73B99C5BBB93D251DF9731B9C5D824F30CDCE (TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C* __this, int32_t ___0_millisecondsTimeout, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t TimeoutTracker_get_RemainingMilliseconds_mDAA8A0E545F8E28D3688095CA346B0A04337BC23 (TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool TimeoutTracker_get_IsExpired_m5E08DCC01117EF66ADEBA5472B27833577289813 (TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C* __this, const RuntimeMethod* method) ;
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void TimeoutTracker__ctor_mBFE73B99C5BBB93D251DF9731B9C5D824F30CDCE (TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C* __this, int32_t ___0_millisecondsTimeout, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&TimeoutTracker__ctor_mBFE73B99C5BBB93D251DF9731B9C5D824F30CDCE_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	DECLARE_METHOD_EXEC_CTX(methodExecutionContext, TimeoutTracker__ctor_mBFE73B99C5BBB93D251DF9731B9C5D824F30CDCE_RuntimeMethod_var, NULL, NULL, NULL);
	CHECK_PAUSE_POINT;
	{
		int32_t L_0 = ___0_millisecondsTimeout;
		if ((((int32_t)L_0) >= ((int32_t)(-1))))
		{
			goto IL_000f;
		}
	}
	{
		ArgumentOutOfRangeException_tEA2822DAF62B10EEED00E0E3A341D4BAF78CF85F* L_1 = (ArgumentOutOfRangeException_tEA2822DAF62B10EEED00E0E3A341D4BAF78CF85F*)il2cpp_codegen_object_new(((RuntimeClass*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&ArgumentOutOfRangeException_tEA2822DAF62B10EEED00E0E3A341D4BAF78CF85F_il2cpp_TypeInfo_var)));
		ArgumentOutOfRangeException__ctor_mBC1D5DEEA1BA41DE77228CB27D6BAFEB6DCCBF4A(L_1, ((String_t*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&_stringLiteralCF088CD7CED04705BD5E7559DBB7DD9A14559182)), NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_1, ((RuntimeMethod*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&TimeoutTracker__ctor_mBFE73B99C5BBB93D251DF9731B9C5D824F30CDCE_RuntimeMethod_var)));
	}

IL_000f:
	{
		int32_t L_2 = ___0_millisecondsTimeout;
		__this->___m_total = L_2;
		int32_t L_3 = __this->___m_total;
		if ((((int32_t)L_3) == ((int32_t)(-1))))
		{
			goto IL_0033;
		}
	}
	{
		int32_t L_4 = __this->___m_total;
		if (!L_4)
		{
			goto IL_0033;
		}
	}
	{
		int32_t L_5;
		L_5 = Environment_get_TickCount_m183BFCF001E12849E3D898957F0B81FD88BA183C(NULL);
		__this->___m_start = L_5;
		return;
	}

IL_0033:
	{
		__this->___m_start = 0;
		return;
	}
}
IL2CPP_EXTERN_C  void TimeoutTracker__ctor_mBFE73B99C5BBB93D251DF9731B9C5D824F30CDCE_AdjustorThunk (RuntimeObject* __this, int32_t ___0_millisecondsTimeout, const RuntimeMethod* method)
{
	TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C*>(__this + _offset);
	TimeoutTracker__ctor_mBFE73B99C5BBB93D251DF9731B9C5D824F30CDCE(_thisAdjusted, ___0_millisecondsTimeout, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t TimeoutTracker_get_RemainingMilliseconds_mDAA8A0E545F8E28D3688095CA346B0A04337BC23 (TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&TimeoutTracker_get_RemainingMilliseconds_mDAA8A0E545F8E28D3688095CA346B0A04337BC23_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	DECLARE_METHOD_EXEC_CTX(methodExecutionContext, TimeoutTracker_get_RemainingMilliseconds_mDAA8A0E545F8E28D3688095CA346B0A04337BC23_RuntimeMethod_var, NULL, NULL, NULL);
	CHECK_PAUSE_POINT;
	{
		int32_t L_0 = __this->___m_total;
		if ((((int32_t)L_0) == ((int32_t)(-1))))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_1 = __this->___m_total;
		if (L_1)
		{
			goto IL_0018;
		}
	}

IL_0011:
	{
		int32_t L_2 = __this->___m_total;
		return L_2;
	}

IL_0018:
	{
		int32_t L_3;
		L_3 = Environment_get_TickCount_m183BFCF001E12849E3D898957F0B81FD88BA183C(NULL);
		int32_t L_4 = __this->___m_start;
		V_0 = ((int32_t)il2cpp_codegen_subtract(L_3, L_4));
		int32_t L_5 = V_0;
		if ((((int32_t)L_5) < ((int32_t)0)))
		{
			goto IL_0032;
		}
	}
	{
		int32_t L_6 = V_0;
		int32_t L_7 = __this->___m_total;
		if ((((int32_t)L_6) < ((int32_t)L_7)))
		{
			goto IL_0034;
		}
	}

IL_0032:
	{
		return 0;
	}

IL_0034:
	{
		int32_t L_8 = __this->___m_total;
		int32_t L_9 = V_0;
		return ((int32_t)il2cpp_codegen_subtract(L_8, L_9));
	}
}
IL2CPP_EXTERN_C  int32_t TimeoutTracker_get_RemainingMilliseconds_mDAA8A0E545F8E28D3688095CA346B0A04337BC23_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C*>(__this + _offset);
	int32_t _returnValue;
	_returnValue = TimeoutTracker_get_RemainingMilliseconds_mDAA8A0E545F8E28D3688095CA346B0A04337BC23(_thisAdjusted, method);
	return _returnValue;
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool TimeoutTracker_get_IsExpired_m5E08DCC01117EF66ADEBA5472B27833577289813 (TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&TimeoutTracker_get_IsExpired_m5E08DCC01117EF66ADEBA5472B27833577289813_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	DECLARE_METHOD_EXEC_CTX(methodExecutionContext, TimeoutTracker_get_IsExpired_m5E08DCC01117EF66ADEBA5472B27833577289813_RuntimeMethod_var, NULL, NULL, NULL);
	CHECK_PAUSE_POINT;
	{
		int32_t L_0;
		L_0 = TimeoutTracker_get_RemainingMilliseconds_mDAA8A0E545F8E28D3688095CA346B0A04337BC23(__this, NULL);
		return (bool)((((int32_t)L_0) == ((int32_t)0))? 1 : 0);
	}
}
IL2CPP_EXTERN_C  bool TimeoutTracker_get_IsExpired_m5E08DCC01117EF66ADEBA5472B27833577289813_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<TimeoutTracker_tF7165D38C7B863F71E6B60DF267C8DDE0D76299C*>(__this + _offset);
	bool _returnValue;
	_returnValue = TimeoutTracker_get_IsExpired_m5E08DCC01117EF66ADEBA5472B27833577289813(_thisAdjusted, method);
	return _returnValue;
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
