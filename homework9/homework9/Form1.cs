﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace homework9
{
    public partial class Form1 : Form
    {
        BindingSource bdsResult = new BindingSource();
        Crawler crawler = new Crawler();
        
        public Form1()
        {
            InitializeComponent();
            dgvResult.DataSource = bdsResult;
            crawler.PageDownloaded += Crawler_PageDownloaded;
            crawler.CrawlerStopped += Crawler_CrawlerStopped;
        }

        private void Crawler_CrawlerStopped(Crawler obj)
        {
            Action action = () => 
            { 
                lblCondition.Text = "Crawler stopped"; 
            };
            if (this.InvokeRequired) this.Invoke(action);
            else action();
        }

        private void Crawler_PageDownloaded(Crawler crawler, string url, string info)
        {
            Action action = () =>
            {
                var pageInfo = new { Index = bdsResult.Count + 1, URL = url, Status = info };
                bdsResult.Add(pageInfo);
            };
            if (this.InvokeRequired) this.Invoke(action);
            else action();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            bdsResult.Clear();
            
            crawler.StartURL = tbxUrl.Text;
            Match match = Regex.Match(crawler.StartURL, Crawler.urlParseRegex);
            if (match.Length == 0) return;
            string host = match.Groups["host"].Value;
            crawler.HostFilter = "^" + host + "$";
            crawler.FileFilter = "((.html?|.aspx|.jsp|.php)$|^[^.]+$)";
            new Thread(crawler.Start).Start();
            lblCondition.Text = "Crawler started";
        }
    }
}
