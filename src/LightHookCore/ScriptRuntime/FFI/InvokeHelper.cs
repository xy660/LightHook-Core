/*
 * Copyright 2025 xy660
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptRuntime.FFI
{
    public static class InvokeHelper
    {
        //程序化生成调用栈模板代码
        public static unsafe nint func0(nint ptr)
        {
            var func = (delegate* unmanaged<nint>)ptr;
            return func();
        }
        public static unsafe nint func1(nint ptr, nint arg0)
        {
            var func = (delegate* unmanaged<nint, nint>)ptr;
            return func(arg0);
        }
        public static unsafe nint func2(nint ptr, nint arg0, nint arg1)
        {
            var func = (delegate* unmanaged<nint, nint, nint>)ptr;
            return func(arg0, arg1);
        }
        public static unsafe nint func3(nint ptr, nint arg0, nint arg1, nint arg2)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2);
        }
        public static unsafe nint func4(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3);
        }
        public static unsafe nint func5(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4);
        }
        public static unsafe nint func6(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5);
        }
        public static unsafe nint func7(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public static unsafe nint func8(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public static unsafe nint func9(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public static unsafe nint func10(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public static unsafe nint func11(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
        public static unsafe nint func12(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }
        public static unsafe nint func13(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }
        public static unsafe nint func14(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }
        public static unsafe nint func15(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }
        public static unsafe nint func16(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }
        public static unsafe nint func17(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }
        public static unsafe nint func18(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17);
        }
        public static unsafe nint func19(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18);
        }
        public static unsafe nint func20(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19);
        }
        public static unsafe nint func21(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20);
        }
        public static unsafe nint func22(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21);
        }
        public static unsafe nint func23(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22);
        }
        public static unsafe nint func24(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23);
        }
        public static unsafe nint func25(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24);
        }
        public static unsafe nint func26(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25);
        }
        public static unsafe nint func27(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26);
        }
        public static unsafe nint func28(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26, nint arg27)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27);
        }
        public static unsafe nint func29(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26, nint arg27, nint arg28)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27, arg28);
        }
        public static unsafe nint func30(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26, nint arg27, nint arg28, nint arg29)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27, arg28, arg29);
        }
        public static unsafe nint func31(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26, nint arg27, nint arg28, nint arg29, nint arg30)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27, arg28, arg29, arg30);
        }
        public static unsafe nint func32(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26, nint arg27, nint arg28, nint arg29, nint arg30, nint arg31)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint>)ptr;
            return func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27, arg28, arg29, arg30, arg31);
        }
        public static unsafe void act0(nint ptr)
        {
            var func = (delegate* unmanaged<void>)ptr;
            func();
        }
        public static unsafe void act1(nint ptr, nint arg0)
        {
            var func = (delegate* unmanaged<nint, void>)ptr;
            func(arg0);
        }
        public static unsafe void act2(nint ptr, nint arg0, nint arg1)
        {
            var func = (delegate* unmanaged<nint, nint, void>)ptr;
            func(arg0, arg1);
        }
        public static unsafe void act3(nint ptr, nint arg0, nint arg1, nint arg2)
        {
            var func = (delegate* unmanaged<nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2);
        }
        public static unsafe void act4(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3);
        }
        public static unsafe void act5(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4);
        }
        public static unsafe void act6(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5);
        }
        public static unsafe void act7(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public static unsafe void act8(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public static unsafe void act9(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public static unsafe void act10(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public static unsafe void act11(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
        public static unsafe void act12(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }
        public static unsafe void act13(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }
        public static unsafe void act14(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }
        public static unsafe void act15(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }
        public static unsafe void act16(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }
        public static unsafe void act17(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }
        public static unsafe void act18(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17);
        }
        public static unsafe void act19(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18);
        }
        public static unsafe void act20(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19);
        }
        public static unsafe void act21(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20);
        }
        public static unsafe void act22(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21);
        }
        public static unsafe void act23(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22);
        }
        public static unsafe void act24(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23);
        }
        public static unsafe void act25(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24);
        }
        public static unsafe void act26(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25);
        }
        public static unsafe void act27(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26);
        }
        public static unsafe void act28(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26, nint arg27)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27);
        }
        public static unsafe void act29(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26, nint arg27, nint arg28)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27, arg28);
        }
        public static unsafe void act30(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26, nint arg27, nint arg28, nint arg29)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27, arg28, arg29);
        }
        public static unsafe void act31(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26, nint arg27, nint arg28, nint arg29, nint arg30)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27, arg28, arg29, arg30);
        }
        public static unsafe void act32(nint ptr, nint arg0, nint arg1, nint arg2, nint arg3, nint arg4, nint arg5, nint arg6, nint arg7, nint arg8, nint arg9, nint arg10, nint arg11, nint arg12, nint arg13, nint arg14, nint arg15, nint arg16, nint arg17, nint arg18, nint arg19, nint arg20, nint arg21, nint arg22, nint arg23, nint arg24, nint arg25, nint arg26, nint arg27, nint arg28, nint arg29, nint arg30, nint arg31)
        {
            var func = (delegate* unmanaged<nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void>)ptr;
            func(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27, arg28, arg29, arg30, arg31);
        }
        public static nint CallPtr(nint ptr, nint[] args, bool noReturn)
        {
            if (noReturn)
            {
                switch (args.Length)
                {
                    case 0: act0(ptr); break;
                    case 1: act1(ptr, args[0]); break;
                    case 2: act2(ptr, args[0], args[1]); break;
                    case 3: act3(ptr, args[0], args[1], args[2]); break;
                    case 4: act4(ptr, args[0], args[1], args[2], args[3]); break;
                    case 5: act5(ptr, args[0], args[1], args[2], args[3], args[4]); break;
                    case 6: act6(ptr, args[0], args[1], args[2], args[3], args[4], args[5]); break;
                    case 7: act7(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6]); break;
                    case 8: act8(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]); break;
                    case 9: act9(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]); break;
                    case 10: act10(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]); break;
                    case 11: act11(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]); break;
                    case 12: act12(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]); break;
                    case 13: act13(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]); break;
                    case 14: act14(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]); break;
                    case 15: act15(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]); break;
                    case 16: act16(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]); break;
                    case 17: act17(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16]); break;
                    case 18: act18(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17]); break;
                    case 19: act19(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18]); break;
                    case 20: act20(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19]); break;
                    case 21: act21(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20]); break;
                    case 22: act22(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21]); break;
                    case 23: act23(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22]); break;
                    case 24: act24(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23]); break;
                    case 25: act25(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24]); break;
                    case 26: act26(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25]); break;
                    case 27: act27(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26]); break;
                    case 28: act28(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26], args[27]); break;
                    case 29: act29(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26], args[27], args[28]); break;
                    case 30: act30(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26], args[27], args[28], args[29]); break;
                    case 31: act31(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26], args[27], args[28], args[29], args[30]); break;
                    case 32: act32(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26], args[27], args[28], args[29], args[30], args[31]); break;
                }
            }
            else
            {
                switch (args.Length)
                {
                    case 0: return func0(ptr);
                    case 1: return func1(ptr, args[0]);
                    case 2: return func2(ptr, args[0], args[1]);
                    case 3: return func3(ptr, args[0], args[1], args[2]);
                    case 4: return func4(ptr, args[0], args[1], args[2], args[3]);
                    case 5: return func5(ptr, args[0], args[1], args[2], args[3], args[4]);
                    case 6: return func6(ptr, args[0], args[1], args[2], args[3], args[4], args[5]);
                    case 7: return func7(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                    case 8: return func8(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                    case 9: return func9(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                    case 10: return func10(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                    case 11: return func11(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                    case 12: return func12(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                    case 13: return func13(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                    case 14: return func14(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                    case 15: return func15(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                    case 16: return func16(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
                    case 17: return func17(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16]);
                    case 18: return func18(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17]);
                    case 19: return func19(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18]);
                    case 20: return func20(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19]);
                    case 21: return func21(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20]);
                    case 22: return func22(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21]);
                    case 23: return func23(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22]);
                    case 24: return func24(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23]);
                    case 25: return func25(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24]);
                    case 26: return func26(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25]);
                    case 27: return func27(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26]);
                    case 28: return func28(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26], args[27]);
                    case 29: return func29(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26], args[27], args[28]);
                    case 30: return func30(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26], args[27], args[28], args[29]);
                    case 31: return func31(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26], args[27], args[28], args[29], args[30]);
                    case 32: return func32(ptr, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15], args[16], args[17], args[18], args[19], args[20], args[21], args[22], args[23], args[24], args[25], args[26], args[27], args[28], args[29], args[30], args[31]);
                }
            }
            return 0;
        }
    }


}
