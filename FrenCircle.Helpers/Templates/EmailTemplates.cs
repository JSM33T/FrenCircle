namespace FrenCircle.Helpers.Templates;

public class EmailTemplates
{
    public static string OtpTemplate => @"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>OTP Email</title>
  <style>
    body {
      font-family: Arial, sans-serif;
      background-color: #f4f4f4;
      margin: 0;
      padding: 0;
    }
    .container {
      max-width: 600px;
      margin: 20px auto;
      background-color: #ffffff;
      padding: 20px;
      border-radius: 8px;
      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    }
    .header {
      text-align: center;
      margin-bottom: 20px;
    }
    .header img {
      max-width: 150px;
    }
    .otp-code {
      text-align: center;
      font-size: 24px;
      font-weight: bold;
      color: #333333;
      margin: 20px 0;
      padding: 10px;
      background-color: #f9f9f9;
      border: 1px solid #dddddd;
      border-radius: 4px;
    }
    .footer {
      text-align: center;
      margin-top: 20px;
      font-size: 12px;
      color: #777777;
    }
    .footer a {
      color: #007BFF;
      text-decoration: none;
    }
  </style>
</head>
<body>
  <div class=""container"">
    <!-- Header Section -->
    <div class=""header"">
      <img src=""https://frencircle.com/media/logo.png"" alt=""Your Company Logo"">
    </div>

    <!-- OTP Section -->
    <p>Hi {{USERNAME}},</p>
    <p>Your One-Time Password (OTP) for verification is:</p>
    <div class=""otp-code"">{{OTP}}</div>
    <p>This code is valid for <strong>30 minutes</strong>. Do not share it with anyone.</p>

    <!-- Footer Section -->
    <div class=""footer"">
      <p>If you didn’t request this OTP, please contact us immediately at <a href=""mailto:mail@frencircle.com"">mail@frencirlce.com</a>.</p>
      <p>&copy; FrenCirlce. All rights reserved.</p>
    </div>
  </div>
</body>
</html>";
}