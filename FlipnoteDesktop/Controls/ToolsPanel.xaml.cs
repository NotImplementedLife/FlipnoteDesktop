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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlipnoteDesktop.Controls
{
    /// <summary>
    /// Interaction logic for ToolsPanel.xaml
    /// </summary>
    public partial class ToolsPanel : UserControl
    {
        public ToolsPanel()
        {
            InitializeComponent();
        }

        public bool isDragging = false;
        public int dX, dY;

        private void DragArea_MouseDown(object sender, MouseButtonEventArgs e)
        {            
            var pt = e.GetPosition(this);
            dX = (int)pt.X;
            dY = (int)pt.Y;
        }

        public FrameCanvasEditor Target;

        private void DragArea_MouseMove(object sender, MouseEventArgs e)
        {
            isDragging = (Mouse.LeftButton == MouseButtonState.Pressed);
            if (isDragging)
            {
                Target.ForceToolBoxDrag(e.GetPosition(this));
            }           
        }

        private void DragArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }
    }            
}