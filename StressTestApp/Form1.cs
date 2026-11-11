using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    // Запускаем столько задач, сколько логических ядер у CPU
                    var tasks = new List<Task>();
                    for (int t = 0; t < Environment.ProcessorCount; t++)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            long localSum = 0;
                            for (int i = 0; i < 100; i++)
                            {
                                if (token.IsCancellationRequested) break;
                                for (int j = 0; j < 50_000_000; j++)
                                    localSum += j;
                            }
                            // Безопасно суммируем результат из разных потоков
                            Interlocked.Add(ref totalSum, localSum);
                        }, token));
                    }
                    Task.WaitAll(tasks.ToArray());
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
        private List<byte[]> _memoryHolder = new List<byte[]>();
        private async void btnMemoryStress_Click(object sender, EventArgs e)
        {
            btnMemoryStress.Enabled = false;
            lblStatus.Text = "Нагрузка на память...";

            var token = cts.Token;

            try
            {
                await Task.Run(() =>
                {
                    for (int i = 0; i < 2000; i++) // 2000 МБ = ~2 ГБ
                    {
                        if (token.IsCancellationRequested) break;

                        byte[] chunk = new byte[1024 * 1024]; // 1 МБ
                                                              // Обязательно «касаемся» каждой страницы — иначе ОС не выделит физическую память
                        for (int k = 0; k < chunk.Length; k += 4096)
                            chunk[k] = 1;
                        _memoryHolder.Add(chunk);

                        this.Invoke(new Action(() =>
                        {
                            progressBar1.Value = (i * 100) / 2000;
                            lblStatus.Text = $"Выделено: {_memoryHolder.Count} МБ";
                        }));
                    }
                }, token);
            }
            catch (OutOfMemoryException)
            {
                lblStatus.Text = "АВАРИЯ: OutOfMemory!";
            }
            finally
            {
                _memoryHolder.Clear(); // ОБЯЗАТЕЛЬНО! Иначе память не вернётся
                btnMemoryStress.Enabled = true;
            }
        }

        private async void btnDiskStress_Click(object sender, EventArgs e)
        {
            btnDiskStress.Enabled = false;
            lblStatus.Text = "Запись файла 1 ГБ...";

            var token = cts.Token;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stress.bin");

            try
            {
                await Task.Run(() =>
                {
                    byte[] buffer = new byte[1024 * 1024];
                    new Random().NextBytes(buffer);

                    using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                    {
                        for (int i = 0; i < 1024; i++) // 1024 блока по 1 МБ = 1 ГБ
                        {
                            if (token.IsCancellationRequested) break;
                            fs.Write(buffer, 0, buffer.Length);

                            this.Invoke(new Action(() =>
                            {
                                progressBar1.Value = (i * 100) / 1024;
                            }));
                        }
                    }

                    // Читаем обратно (нагрузка на чтение + проверка кэша)
                    lblStatus.Text = "Чтение файла обратно...";
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
                    {
                        while (fs.Read(buffer, 0, buffer.Length) > 0)
                        {
                            if (token.IsCancellationRequested) break;
                        }
                    }
                }, token);

                lblStatus.Text = "Диск: 1 ГБ записан и прочитан";
            }
            finally
            {
                try { File.Delete(path); } catch { }
                btnDiskStress.Enabled = true;
            }
        }
    }
}
