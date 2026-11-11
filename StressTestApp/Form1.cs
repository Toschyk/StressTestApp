using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace StressTestApp
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource cts;

        Process currentProcess = Process.GetCurrentProcess();

        public Form1()
        {
            InitializeComponent();
            btnStop.Enabled = false;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            lblStatus.Text = "Выполняется расчет...";
            progressBar1.Value = 0;

            // Инициализируем источник токена
            cts = new CancellationTokenSource();
            var token = cts.Token;

            try
            {
                long result = await Task.Run(() =>
                {
                    long totalSum = 0;
                    for (int i = 0; i < 100; i++)
                    {
                        // Проверяем, не нажал ли пользователь "Остановить"
                        if (token.IsCancellationRequested)
                        {
                            // Выбрасываем исключение, чтобы прервать задачу
                            token.ThrowIfCancellationRequested();
                        }

                        for (int j = 0; j < 50_000_000; j++)
                        {
                            totalSum += j;
                        }

                        this.Invoke(new Action(() => {
                            progressBar1.Value = i + 1;
                        }));
                    }
                    return totalSum;
                }, token); // Передаем токен в Task

                lblStatus.Text = "Готово! Сумма: " + result;
            }
            catch (OperationCanceledException)
            {
                // Перехватываем исключение отмены
                lblStatus.Text = "Операция отменена пользователем.";
            }
            finally
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                cts.Dispose();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel(); // Подаем сигнал на отмену
            }
        }

        private void timerMetrics_Tick(object sender, EventArgs e)
        {
            currentProcess.Refresh();

            // Потребление памяти (в Мегабайтах)
            long ramMb = currentProcess.WorkingSet64 / (1024 * 1024);

            // Загрузка CPU (приблизительная, зависит от количества ядер)
            // В реальном проекте считается сложнее, но для демонстрации сойдет
            TimeSpan cpuTime = currentProcess.TotalProcessorTime;

            lblMetrics.Text = $"RAM: {ramMb} МБ | Потоков: {currentProcess.Threads.Count}";
        }
    }
}
