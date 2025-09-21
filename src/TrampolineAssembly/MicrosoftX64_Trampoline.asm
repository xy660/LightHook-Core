bits 64

;调用方：MicrosoftX64 ABI，目标函数：MicrosoftX64 ABI
;适用平台：Windows X64

;*((nint*)(p + 2)) = entryFunc;
;*((nint*)(p + 12)) = heapBuffer;
;*((nint*)(p + 22)) = funcid;

mov r10, 10000000000h ; 保留修补点
mov r11,10000000000h ;堆内存修补点
mov rax,10000000000h ;func_id修补点

push r12 ;保存一下非易失性寄存器

mov qword [r11],rcx ;arg1

mov qword [r11 + 8],rdx ;arg2

mov qword [r11 + 16],r8 ;arg3

mov qword [r11 + 24],r9 ;arg4

mov r12,qword [rsp+48] ;arg5
mov qword [r11 + 32],r12

mov r12,qword [rsp+56] ;arg6
mov qword [r11 + 40],r12

mov r12,qword [rsp+64] ;arg7
mov qword [r11 + 48],r12

mov r12,qword [rsp+72] ;arg8
mov qword [r11 + 56],r12

mov r12,qword [rsp+80] ;arg9
mov qword [r11 + 64],r12

mov r12,qword [rsp+88] ;arg10
mov qword [r11 + 72],r12

mov r12,qword [rsp+96] ;arg11
mov qword [r11 + 80],r12

mov r12,qword [rsp+104] ;arg12
mov qword [r11 + 88],r12

mov rcx,rax ;arg1==func_id
mov rdx,r11 ;arg2=argv*
pop r12 ;恢复非易失性寄存器
jmp r10
