namespace QuanLyNhaHang
{
    internal static class Program
    {
        public static string TaiKhoanDangNhap = "";
        public static string VaiTroDangNhap = "";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormLogin fLogin = new FormLogin();
            if (fLogin.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FormMain());
            }
        }
    }
}
