// Buildした際にDebug.LogやAssertを無効にする

#if NO_DEBUG

using UnityEngine;
using System.Diagnostics;

public static class Debug
{
	//DUMMYは定義しなくて良い。
	[Conditional("DUMMY")] public static void Assert(bool condition, string message, Object context) {}
	[Conditional("DUMMY")] public static void Assert(bool condition, object message, Object context) {}
	[Conditional("DUMMY")] public static void Assert(bool condition, string message) {}
	[Conditional("DUMMY")] public static void Assert(bool condition, object message) {}
	[Conditional("DUMMY")] public static void Assert(bool condition, Object context) {}
	[Conditional("DUMMY")] public static void Assert(bool condition) {}
	[Conditional("DUMMY")] public static void Assert(bool condition, string format, params object[] args) {}
	[Conditional("DUMMY")] public static void AssertFormat(bool condition, string format, params object[] args) {}
	[Conditional("DUMMY")] public static void AssertFormat(bool condition, Object context, string format, params object[] args) {}
	[Conditional("DUMMY")] public static void Break() {}
	[Conditional("DUMMY")] public static void ClearDeveloperConsole() {}
	[Conditional("DUMMY")] public static void DebugBreak() {}
	[Conditional("DUMMY")] public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest) {}
	[Conditional("DUMMY")] public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration) {}
	[Conditional("DUMMY")] public static void DrawLine(Vector3 start, Vector3 end) {}
	[Conditional("DUMMY")] public static void DrawLine(Vector3 start, Vector3 end, Color color) {}
	[Conditional("DUMMY")] public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) {}
	[Conditional("DUMMY")] public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest) {}
	[Conditional("DUMMY")] public static void DrawRay(Vector3 start, Vector3 dir) {}
	[Conditional("DUMMY")] public static void DrawRay(Vector3 start, Vector3 dir, Color color) {}
	[Conditional("DUMMY")] public static void Log(object message) {}
	[Conditional("DUMMY")] public static void Log(object message, Object context) {}
	[Conditional("DUMMY")] public static void LogAssertion(object message, Object context) {}
	[Conditional("DUMMY")] public static void LogAssertion(object message) {}
	[Conditional("DUMMY")] public static void LogAssertionFormat(Object context, string format, params object[] args) {}
	[Conditional("DUMMY")] public static void LogAssertionFormat(string format, params object[] args) {}
	[Conditional("DUMMY")] public static void LogError(object message, Object context) {}
	[Conditional("DUMMY")] public static void LogError(object message) {}
	[Conditional("DUMMY")] public static void LogErrorFormat(string format, params object[] args) {}
	[Conditional("DUMMY")] public static void LogErrorFormat(Object context, string format, params object[] args) {}
	[Conditional("DUMMY")] public static void LogException(System.Exception exception, Object context) {}
	[Conditional("DUMMY")] public static void LogException(System.Exception exception) {}
	[Conditional("DUMMY")] public static void LogFormat(Object context, string format, params object[] args) {}
	[Conditional("DUMMY")] public static void LogFormat(string format, params object[] args) {}
	[Conditional("DUMMY")] public static void LogWarning(object message) {}
	[Conditional("DUMMY")] public static void LogWarning(object message, Object context) {}
	[Conditional("DUMMY")] public static void LogWarningFormat(string format, params object[] args) {}
	[Conditional("DUMMY")] public static void LogWarningFormat(Object context, string format, params object[] args) {}
}

#endif