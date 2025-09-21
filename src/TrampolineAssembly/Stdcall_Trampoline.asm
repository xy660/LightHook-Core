bits 32

;调用方：stdcall，目标函数：cdecl
;适用平台：Unix/Windows X86

;*((nint*)(p + 4)) = targetFunc;
;*((nint*)(p + 9)) = heapBuf;
;*((nint*)(p + 14)) = func_id;
;*((nint*)(p + 19)) = argCount;

push ebx ;esp+20
push esi ;esp+16
push edi ;esp+12

push 1000000h ; 保留修补点 esp+8
mov ecx,1000000h ;堆内存修补点
push 1000000h ;func_id修补点  esp+4
push 1000000h ;argCount esp

mov ebx,dword[esp] ;ebx=argCount
lea esi,[esp+28] ;获取栈上指向第一个参数的地址
mov edi,ecx ;临时heap指针
_loop_start:
cmp ebx,0 ;while(argCount != 0){storge;argCount--}
je near _end_loop

mov eax,dword[esi] ;eax=*argv
mov dword[edi],eax ;*heap=eax
add esi,4 ;argv++
add edi,4 ;heap++
sub ebx,1 ;argCount--

jmp near _loop_start

_end_loop:


pop edi ;argCount
pop edx ;edx=func_id
pop eax ;eax=targetFunc*

push ecx
push edx
call eax ;调用处理函数，清理返回（目标函数cdecl）
pop edx
pop ecx

mov edx,edi ;转移到易失性寄存器

pop edi
pop esi
pop ebx ;恢复非易失性寄存器

pop ecx  ;保存返回地址
_clean_start: ;开始清理栈
cmp edx,0
je near _clean_end
add esp,4 ;循环弹出dword
sub edx,1 ;i--
jmp _clean_start

_clean_end:


push ecx ;压入返回地址
ret