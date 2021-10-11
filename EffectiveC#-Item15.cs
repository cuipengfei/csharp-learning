using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace DefaultNamespace
{
    public class EffectiveC__Item15
    {
        
        // 这样写将导致每次调用OnPaint方法， 都会创建一个 Font 对象
        // 但其实每次创建出的对象都是同样的设定
        private static void OnPaint(PaintEventArgs e)
        {
            var myFont = new Font("Arial", 10.0f);
            var black = Brushes.Black;
            e.Graphics.DrawString(DateTime.Now.ToString(),myFont,black,new Point(0,0));
        }
        
        // 更好的方法是将这种对象， 由局部变量改为成员变量
        //就可以每次都使用同一个Font对象，不需要频繁的执行GC机制

        private readonly Font _myFont = new("Arial", 10.0f);
        private void OnPaintEffective(PaintEventArgs e)
        {
            e.Graphics.DrawString(DateTime.Now.ToString(),_myFont,Brushes.Black,new Point(0,0));
        }

        public void Paint(PaintEventArgs e)
        {
            OnPaint(e); 
            OnPaintEffective(e);
        }

        private static Brush _blackBrush;

        public static Brush Black
        {
            get
            {
                if (_blackBrush == null)
                {
                    _blackBrush = new SolidBrush(Color.Black);
                }

                return _blackBrush;
            }
        }
        
        // 不可变类型
        //修改String其实是创建了新的String对象，替代原本的String对象
        
        private string ModifyString(string str)
        {
            str += "Good ";
            str += "Performance";
            return str;
        }
        
        private string ActuallyModifyString(string str)
        {
            string temp1 = new string(str + "Good ");
            str = temp1;
            string temp2 = new string(str + "Performance");
            str = temp2;
            return str;
        }
        
        // 更好的方法是通过内插字符串
        private string BetterModifyStringWay(string str)
        {
            return $"{str}Good Performance";
        }
        
        // 通过StringBuilder来实现
        private string MuchBetterModifyStringWay(string str)
        {
            var msg = new StringBuilder(str);
            msg.Append("Good ");
            msg.Append("Performance");
            return msg.ToString();
        }
    }
}