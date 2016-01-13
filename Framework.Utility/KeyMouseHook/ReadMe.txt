键盘、鼠标钩子用法
1，加载钩子
                Hook  hook = Hook.GetInstance();
                hook.OnKeyPress += new KeyEventHandler(DealKeyUp);
                hook.InstallKeyBoardHook();

2，卸载钩子
       if (hook != null){
	     hook.UninstallKeyBoardHook();
	   }
                  