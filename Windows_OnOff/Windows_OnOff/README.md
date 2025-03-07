# :pencil: Description
This Windows Service is programmed for personal needs, which communicates with the computer via Telegram bot, to alert the user when the computer is started, shut down, unlocked or locked (In general, if the Windows service is turned off or on, it can be improved to be more accurate if necessary in the future) via telegram bot
![Screenshot 2025-02-07 162712](https://github.com/user-attachments/assets/b4411066-4ab3-4331-9c73-097709b56cda)
![Screenshot 2025-02-07 16340tttttttt1](https://github.com/user-attachments/assets/5b99242b-94fa-4b04-bf0a-9ce8de23c99d)
# :octopus: Telegram Bot
you have to create a Telegram bot and enter your chatid and token in variables telegram_botToken and telegram_chatId </br>
```csharp
   public const string telegram_botToken = "Your Token";
   private const string telegram_chatId = "Your ChatId";
```

# :gear: Usage
This service can perform many tasks through certain commands, for example sending or get a file (video images, etc.).
![Screenshot 2025-02-07 181036ffffffffffffffffffffff](https://github.com/user-attachments/assets/78ff898a-2419-4a60-8a73-6a069ddb7900)

by writing the device name, then the user name, then the command to be executed, then this Caret ^
#### :keyboard: Commands explanation
  **shutdown** "To shut down the Computer"</br>
  **sleep** "To sleep the Computer"</br>
  **hibernate** "To hibernate the Computer" </br>
  **server_info** "Mostly for testing"</br>
  **s_stop** "To stop the service" </br>
  **s_uninstall** "To uninstall the service" </br>
  **s_delete** "To delete the service"</br>
  **get_f** "To get all Folders and Files example (username^ get_f:C:\Users\username\Desktop)" </br>
  **get_folder** "To get only all Folders example (username^ get_folder:C:\Users\username\Desktop)"</br>
  **get_file** "To get only all Files example (username^ get_file:C:\Users\username\Desktop)"</br>
  **get_p** "To get a photo example (username^ get_p:C:\Users\username\Desktop\image.jpg)"</br>
  **get_v** "To get a Video example (username^ get_v:C:\Users\username\Desktop\video.mp4)"</br>
  **get_d** "To get a document example (username^ get_d:C:\Users\username\Desktop\document.pdf)"</br>
  **set_p** "To download in the Computer a Image example (username^ set_p:C:\Users\username\Desktop\Image.pdf), if invalid path then the photo will be saved in path of Windows Service"</br>
  **set_v** "To download in the Computer a Video example (username^ set_p:C:\Users\username\Desktop\Video.pdf), if invalid path then the Video will be saved in path of Windows Service"</br>
  **set_d** "To download in the Computer a Document example (username^ set_p:C:\Users\username\Desktop\document.pdf), if invalid path then the Document will be saved in path of Windows Service"</br>
  **command_info** "get info to all Commands"</br>

# :zap: Easy to Install via service_install.bat Script
With service_install.bat Script you can install the service with just one click
![Screenshot 2025-02-07 181429](https://github.com/user-attachments/assets/2b77ec3f-3e89-4a1a-9f96-b0bc501bc7ed)

With script you can also simply start, stop or delete
![Screenshot 2025-02-07 1914qqqqqqqqqqqqqqqqqqqq37](https://github.com/user-attachments/assets/070fc6ef-5ad8-4c24-8f38-144287c526dc)

# :hammer_and_wrench: Task Scheduler
The service create a task in "Task Scheduler" which ensures that the service at Windows startup, 
and every hour it is checked that the service is running

# Support
If you are having problems, please let me know by [raising a new issue](https://github.com/nedalWahhoud/Windows_OnOff/issues).


# :scroll: Disclaimer
Avalonia is licensed under the [MIT License](https://github.com/nedalWahhoud/Windows_OnOff/blob/main/License).</br>
