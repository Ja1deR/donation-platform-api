using System;

namespace Software_2.Services
{
    public class EmailTemplateService
    {
        public string ConstruirMensajeLogin(string nombreUsuario)
        {
            string logoBase64 = ImageToBase64("Helpers/ImagesEmail/logo.png");

            return $@"
    <html>
        <body style='font-family: Arial; padding: 20px;'>
            <div style='max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 10px; padding: 20px;'>
                <h2 style='color: #2c3e50; text-align: center;'>¡Bienvenido {nombreUsuario}!</h2>
                <p style='text-align: center; color: #7f8c8d;'>Has iniciado sesión exitosamente en Rescate Solidario</p>
                <p style='text-align: center; color: #7f8c8d;'>Fecha: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}</p>
                <hr style='border-color: #f0f0f0;'>
                <p style='text-align: center; font-size: 0.9em; color: #bdc3c7;'>
                    Si no reconoces esta actividad, por favor contacta con soporte.
                </p>
            </div>
        </body>
    </html>";
        }

        private string ImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }
 
        public string ConstruirMensajeDonacion(
            string nombreUsuario,
            int cantidad,
            string nombreFundacion,
            string nombrePublicacion)
        {
            return $@"
        <h1 style='color: #2c3e50;'>¡Gracias por tu generosidad, {nombreUsuario}!</h1>
        <p>Has realizado una donación de <strong>{cantidad}</strong> unidades a la fundación <strong>{nombreFundacion}</strong>.</p>
        <p>Destinado a: <em>{nombrePublicacion}</em></p>
        <p>Fecha de la donación: {DateTime.Now:dd/MM/yyyy HH:mm}</p>
        <hr>
        <p>Este es un comprobante automático, no es necesario responder.</p>
    ";
        }
        public string ConstruirMensajeCambioEstado(
            string nombreDonante,
            int cantidad,
            string nombreFundacion,
            string estadoAnterior,
            string nuevoEstado)
        {
            return $@"
    <html>
        <body style='font-family: Arial; padding: 20px;'>
            <div style='max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 10px; padding: 20px;'>
                <h2 style='color: #2c3e50;'>¡Hola {nombreDonante}!</h2>
                <p>El estado de tu donación de <strong>{cantidad} unidades</strong> a <strong>{nombreFundacion}</strong> ha cambiado:</p>
                <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                    <p style='margin: 0;'><strong>Estado anterior:</strong> {estadoAnterior}</p>
                    <p style='margin: 0;'><strong>Nuevo estado:</strong> {nuevoEstado}</p>
                </div>
                <p style='color: #6c757d;'>Fecha de actualización: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}</p>
            </div>
        </body>
    </html>";
        }
    }
}
