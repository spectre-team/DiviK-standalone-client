﻿/*
 * MainWindow.xaml.cs
 * Contains interaction logic for MainWindow.xml WPF Divik client.
 *
   Copyright 2017 Michal Wolny, Grzegorz Mrukwa

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System.Windows;
using Ninject;
using Spectre.DivikWpfClient.ViewModel;
using Spectre.Service;
using Spectre.Service.Abstract;

namespace Spectre.DivikWpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IServiceFactory>()
                .To<ServiceFactory>();
            var viewModel = (MainPageVm)DataContext;
            viewModel.ServiceFactory = ninjectKernel.Get<IServiceFactory>();
        }
    }
}
