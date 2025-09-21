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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScriptRuntime.FFI
{
    public unsafe static class NativeMemoryManager
    {
        [DllImport("libc", SetLastError = true)]
        public static extern void* mmap(void* addr, nuint length, int prot, int flags, int fd, nuint offset);

        [DllImport("libc", SetLastError = true)]
        public static extern int munmap(void* addr, nuint length);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void* VirtualAlloc(void* lpAddress, nuint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualFree(void* lpAddress, nuint dwSize, uint dwFreeType);

        public static void* Alloc(nuint size, nuint alignment)
        {
            if (OperatingSystem.IsWindows())
            {
                return VirtualAlloc(null, size, 0x1000 | 0x2000, 0x40); // MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE
            }
            else
            {
                return mmap(null, size, 0x1 | 0x2 | 0x4, 0x02 | 0x20, -1, 0); // PROT_READ | PROT_WRITE | PROT_EXEC, MAP_PRIVATE | MAP_ANONYMOUS
            }
        }

        public static void Free(void* ptr)
        {
            if (OperatingSystem.IsWindows())
            {
                VirtualFree(ptr, 0, 0x8000); // MEM_RELEASE
            }
            else
            {
                munmap(ptr, 0);
            }
        }
    }
}
