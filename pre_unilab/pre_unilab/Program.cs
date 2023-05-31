namespace pre_unilab
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new スタート画面());
        }
    }

    public partial class Form1 : Form
    {
        #region フィールド変数

        /// <summary>
        /// スレッド分割用
        /// </summary>
        Thread drawThread;

        #endregion

        #region スレッド分割用関数

       

        private void InterThreadRefresh(Action _function)
        {
            try
            {
                if (InvokeRequired) Invoke(_function);
                else _function();
            }
            catch (ObjectDisposedException)
            {
            }
        }

        #endregion
    }
}