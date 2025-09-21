bits 64

; 调用方：System V，目标函数：System V
; 适用平台：Unix x64

;*((nint*)(p + 2)) = pf;
;*((nint*)(p + 12)) = (nint)buf;
;*((nint*)(p + 22)) = funcid;

mov r10, 10000000000h ; 保留修补点
mov r11,10000000000h ;堆内存修补点
mov rax,10000000000h ;func_id修补点

push r12 ; 保存非易失性寄存器

; 将参数存入heap
mov qword [r11],rdi ;arg1
mov qword [r11+8],rsi ;arg2
mov qword [r11+16],rdx ;arg3
mov qword [r11+24],rcx ;arg4
mov qword [r11+32],r8 ;arg5
mov qword [r11+40],r9 ;arg6

;处理栈上的参数
mov r12, qword [rsp+8+8]  ; arg7
mov qword [r11+48], r12
mov r12, qword [rsp+16+8] ; arg8
mov qword [r11+56], r12
mov r12, qword [rsp+24+8] ; arg9
mov qword [r11+64], r12
mov r12, qword [rsp+32+8] ; arg10
mov qword [r11+72], r12
mov r12, qword [rsp+40+8] ; arg11
mov qword [r11+80], r12
mov r12, qword [rsp+48+8] ; arg12
mov qword [r11+88], r12

; 调用目标函数（System V 风格）
mov rdi, rax  ; arg1 = func_id
mov rsi, r11  ; arg2 = argv*
pop r12       ; 恢复非易失性寄存器
jmp r10       ; 跳转到目标函数