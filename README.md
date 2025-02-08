# :pencil: Description
This Windows Service is programmed for personal needs, which communicates with the computer via Telegram bot, to alert the user when the computer is started, shut down, unlocked or locked (In general, if the Windows service is turned off or on, it can be improved to be more accurate if necessary in the future) via telegram bot
![Screenshot 2025-02-07 162712](https://github.com/user-attachments/assets/48c455bc-3e68-4cbe-89bf-2c91f3cd5142)
![Screenshot 2025-02-07 16340tttttttt1](https://github.com/user-attachments/assets/7c30f2db-6a0d-4733-8ea0-a4238a09c794)
# :gear: Usage
This service can perform many tasks through certain commands, for example sending or get a file (video images, etc.).
![Screenshot 2025-02-07 181036ffffffffffffffffffffff](https://github.com/user-attachments/assets/4d9034fd-5c21-4865-b1e1-d2cc983a6b18)
by writing the device name, then the user name, then the command to be executed, then this Caret ^
#### :keyboard: Commands explanation
  **shutdown** "To shut down the Computer"
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
![Screenshot 2025-02-07 181429](https://github.com/user-attachments/assets/a353c32f-9961-4d9c-92ca-a7da57f7998d)
With script you can also simply start, stop or delete
![Screenshot 2025-02-07 1914qqqqqqqqqqqqqqqqqqqq37](https://github.com/user-attachments/assets/90bea368-f6c1-4c24-81b5-b5d2e8a3b0f9)
# :hammer_and_wrench: Task Scheduler
The service create a task in "Task Scheduler" which ensures that the service at Windows startup, 
and every hour it is checked that the service is running
# :scroll: Disclaimer
This Windows service is for personal use only, not for commercial use, and we are not responsible for any unethical or wrong use of this Windows service.
This Windows Service is provided "as is", without warranty of any kind, express or implied. The authors and contributors are not responsible for any issues, damages, or losses resulting from the use of this code.
By using this Windows Service, you agree that you do so at your own risk. Always review and test the code before deploying it in production.



