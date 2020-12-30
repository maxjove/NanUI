var  WinningBrowerWindow =  WinningBrowerWindow || {};


(function ($this) {

    
     $this.close = () => {
        native function Close();
        return Close();
    };

     $this.minimize = () => {
        native function Minimize();
        return Minimize();
    };
     $this.maximize = () => {
        native function Maximize();
        return Maximize();
    };
     $this.restore = () => {
        native function Restore();
        return Restore();
    };
    
  


})(WinningBrowerWindow);


