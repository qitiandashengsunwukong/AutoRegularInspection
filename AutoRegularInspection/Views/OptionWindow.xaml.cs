﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace AutoRegularInspection.Views
{
    /// <summary>
    /// OptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow : Window
    {
        public OptionWindow()
        {
            InitializeComponent();

            var config = XDocument.Load(@"Option.config");
            try
            {
                var pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
                PictureWidth.Text = pictureWidth.Value.ToString();
                var pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();
                PictureHeight.Text = pictureHeight.Value.ToString();
            }
            catch (Exception ex)
            {
                //TODO:数据格式不正确时的异常处理
                throw ex;
            }

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var config = XDocument.Load(@"Option.config");

                var pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
                pictureWidth.Value = PictureWidth.Text;
                var pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();
                pictureHeight.Value = PictureHeight.Text;
                config.Save(@"Option.config");

                MessageBox.Show("保存设置成功！");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
