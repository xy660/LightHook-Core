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
using System.Runtime.InteropServices;

namespace ScriptRuntime.Runtime
{

    public static class NativeThread
    {
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        private static readonly bool IsMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        // Windows
        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        // macOS
        [DllImport("libc")]
        private static extern int pthread_threadid_np(IntPtr thread, out ulong threadId);

        // Linux syscall
        [DllImport("libc", EntryPoint = "syscall", SetLastError = true)]
        private static extern int syscall(int id);

        // pthread fallback
        [DllImport("libc")]
        private static extern ulong pthread_self();

        // Linux gettid (alternative)
        [DllImport("libc", EntryPoint = "gettid", SetLastError = true)]
        private static extern int gettid();

        public static ulong GetCurrentNativeThreadId()
        {
            if (IsWindows)
            {
                return GetCurrentThreadId();
            }
            else if (IsLinux)
            {
                try
                {
                    // First try gettid function
                    int tid = gettid();
                    if (tid != -1) return (ulong)tid;
                }
                catch { }

                // Syscall 备份
                int SYS_gettid =
                    RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? 178 :
                    RuntimeInformation.ProcessArchitecture == Architecture.X64 ? 186 :
                    RuntimeInformation.ProcessArchitecture == Architecture.Arm ? 224 :
                    224; 

                int tidFromSyscall = syscall(SYS_gettid);
                if (tidFromSyscall == -1)
                {
                    return pthread_self(); // 还是失败就调用pthread_self
                }
                return (ulong)tidFromSyscall;
            }
            else if (IsMacOS)
            {
                if (pthread_threadid_np(IntPtr.Zero, out ulong threadId) != 0)
                {
                    return pthread_self(); // 回退
                }
                return threadId;
            }

            throw new PlatformNotSupportedException("线程模型不支持的操作系统");
        }
    }
}
