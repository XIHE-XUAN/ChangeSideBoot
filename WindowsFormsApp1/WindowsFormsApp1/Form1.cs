using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Replace_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey rkMachine = Registry.LocalMachine;
                RegistryKey rkSoftware = rkMachine.OpenSubKey("SOFTWARE",true);
                RegistryKey rkWow6432Node = rkSoftware.OpenSubKey("Wow6432Node",true);
                RegistryKey rkSeewo = rkWow6432Node.OpenSubKey("Seewo",true);
                RegistryKey rkEasiCamera = rkSeewo.OpenSubKey("EasiCamera",true);
                //尝试打开CameraPath，没有则说明第一次打开，此时读取展台原始安装位置。CameraPath放在Wow6432Node下
                RegistryKey CameraPath = rkWow6432Node.OpenSubKey("CameraPath",true);
                //不为空，说明存在，此时直接修改
                if (CameraPath != null)
                {
                    rkEasiCamera.SetValue("ExePath",textBox1.Text);
                    MessageBox.Show($"成功将启动的程序修改为{textBox1.Text}");
                }
                //为空，说明不存在，判断为第一次打开，读取原始安装位置
                else
                {
                    //创建注册表键
                    RegistryKey rkCameraPath = rkWow6432Node.CreateSubKey("CameraPath",true);
                    //获取原始安装位置
                    string Path = rkEasiCamera.GetValue("ExePath") as string;
                    //记录原始安装位置
                    rkCameraPath.SetValue("ExePath",Path);
                    MessageBox.Show("首次启动：获取展台原始安装位置成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey rkMachine = Registry.LocalMachine;
                RegistryKey rkSoftware = rkMachine.OpenSubKey("SOFTWARE",true);
                RegistryKey rkWow6432Node = rkSoftware.OpenSubKey("Wow6432Node",true);
                RegistryKey rkSeewo = rkWow6432Node.OpenSubKey("Seewo",true);
                RegistryKey rkEasiCamera = rkSeewo.OpenSubKey("EasiCamera",true);
                RegistryKey CameraPath = rkWow6432Node.OpenSubKey("CameraPath",true);
                //CameraPath不为空，说明存在，此时读取里面的值并写入到展台ExePath里
                if (CameraPath != null)
                {
                    string Path = CameraPath.GetValue("ExePath") as string;
                    rkEasiCamera.SetValue("ExePath",Path);
                    MessageBox.Show("还原成功");
                    rkEasiCamera.Close();
                    CameraPath.Close();
                }
                //为空，说明1不存在，判断为第一次打开，读取原始安装位置
                else
                {
                    //创建注册表键
                    RegistryKey rkCameraPath = rkWow6432Node.CreateSubKey("CameraPath",true);
                    //获取安装位置
                    string Path = rkEasiCamera.GetValue("ExePath") as string;
                    //记录
                    rkCameraPath.SetValue("ExePath",Path);
                    MessageBox.Show("首次启动：获取展台原始安装位置成功！");
                    rkEasiCamera.Close();
                    rkCameraPath.Close();
                    
                }
                rkMachine.Close();
                rkSoftware.Close();
                rkWow6432Node.Close();
                rkSeewo.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}