using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryEtapa1
{
    public partial class frmFirma : Form
    {
        // Variable para almacenar la posición anterior del mouse durante el dibujo
        private Point puntoAnterior;
        // Bandera para indicar si el mouse está actualmente dibujando
        private bool isMouseDrawing = false;
        // Bitmap utilizado para almacenar la firma del usuario
        private Bitmap firmaBitmap;
        public frmFirma()
        {
            InitializeComponent();
            InitializeFirmaPictureBox();
            // Eventos para los eventos MouseDown, MouseMove y MouseUp del PictureBox
            pctFirma.MouseDown += new MouseEventHandler(pctFirma_MouseDown);
            pctFirma.MouseMove += new MouseEventHandler(pctFirma_MouseMove);
            pctFirma.MouseUp += new MouseEventHandler(pctFirma_MouseUp);
        }
        private void InitializeFirmaPictureBox()
        {
            // Crea un Bitmap del mismo tamaño que el PictureBox
            firmaBitmap = new Bitmap(pctFirma.Width, pctFirma.Height);

            // Asigna el Bitmap como imagen del PictureBox
            pctFirma.Image = firmaBitmap;

            // Obtiene un objeto Graphics para dibujar en el Bitmap
            Graphics g = Graphics.FromImage(pctFirma.Image);

            // Limpia el Bitmap con color blanco
            g.Clear(Color.White);

            // Libera los recursos utilizados por el objeto Graphics
            g.Dispose();
        }

        private void pctFirma_MouseDown(object sender, MouseEventArgs e)
        {
            // Verifica si se hizo clic con el botón izquierdo del mouse
            if (e.Button == MouseButtons.Left)
            {
                // Indica que el mouse está comenzando a dibujar
                isMouseDrawing = true;

                // Almacena la posición inicial del clic
                puntoAnterior = e.Location;
            }
        }

        private void pctFirma_MouseMove(object sender, MouseEventArgs e)
        {
            // Verifica si el mouse está dibujando
            if (isMouseDrawing)
            {
                // Crea un objeto Graphics temporal para dibujar en el Bitmap
                using (Graphics g = Graphics.FromImage(pctFirma.Image))
                {
                    // Habilita el suavizado de líneas para mejorar la apariencia del trazo
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // Crea un lápiz negro de grosor 2 para dibujar
                    using (Pen pen = new Pen(Color.Black, 2))
                    {
                        // Dibuja una línea desde la posición anterior del mouse hasta la posición actual
                        g.DrawLine(pen, puntoAnterior, e.Location);
                    }
                }

                // Actualiza la posición anterior del mouse para el siguiente movimiento
                puntoAnterior = e.Location;

                // Indica al PictureBox que necesita redibujarse
                pctFirma.Invalidate();
            }
        }

        private void pctFirma_MouseUp(object sender, MouseEventArgs e)
        {
            // Indica que el mouse ya no está dibujando
            isMouseDrawing = false;
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Obtiene la fecha y hora actual con formato específico para el nombre del archivo
            string fecha = DateTime.Now.ToString("yyyy.MM.dd / HH:mm:ss") + ".png";

            // Construye la ruta de la carpeta "FIRMA" en el escritorio del usuario
            string carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Firma");

            // Crea la carpeta "FIRMA" si no existe
            Directory.CreateDirectory(carpeta);

            // Combina la fecha y la extensión ".png" para formar el nombre del archivo
            string nombreArchivo = $"{fecha}.png";

            // Construye la ruta completa del archivo en el disco
            string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            // Crea un nuevo Bitmap del mismo tamaño que el PictureBox
            Bitmap bmp = new Bitmap(pctFirma.Width, pctFirma.Height);

            // Dibuja el contenido del PictureBox en el bitmap
            pctFirma.DrawToBitmap(bmp, pctFirma.ClientRectangle);

            // Guarda el bitmap en un archivo
            bmp.Save(rutaCompleta, ImageFormat.Png);

            // Libera los recursos utilizados por el bitmap
            bmp.Dispose();

            // Muestra un mensaje al usuario indicando que la firma se guardó correctamente
            MessageBox.Show("Firma guardada correctamente en escritorio.");
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Obtiene un objeto Graphics para dibujar en el Bitmap
            using (Graphics g = Graphics.FromImage(firmaBitmap))
            {
                // Limpia el Bitmap con color blanco
                g.Clear(Color.White);
            }

            // Invalida el PictureBox para que se actualice la imagen
            pctFirma.Invalidate();
        }
    }
}
