bits 32

;调用方：cdecl，目标函数：cdecl
;适用平台：Unix/Windows X86

;*((nint*)(p + 1)) = entryFunc;
;*((nint*)(p + 6)) = heapBuffer;
;*((nint*)(p + 11)) = func_id;
;*((nint*)(p + 16)) = argCount;

push 1000000h ; 保留修补点 esp+20
mov ecx,1000000h ;堆内存修补点
push 1000000h ;func_id修补点  esp+16
push 1000000h ;argCount esp+12

push ebx ;esp+8
push esi ;esp+4
push edi ;esp
mov ebx,dword[esp+12] ;ebx=argCount
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

pop edi
pop esi
pop ebx ;恢复非易失性寄存器

pop eax ;argCount
pop edx ;edx=func_id
pop eax ;eax=targetFunc*

push ecx
push edx
call eax ;调用处理函数，然后清理返回
pop edx
pop ecx
ret

