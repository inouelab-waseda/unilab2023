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
            Application.Run(new �X�^�[�g���());
        }
    }

    public partial class Form1 : Form
    {
        #region �t�B�[���h�ϐ�

        /// <summary>
        /// �X���b�h�����p
        /// </summary>
        Thread drawThread;

        #endregion

        #region �X���b�h�����p�֐�

       

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