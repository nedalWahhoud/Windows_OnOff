# :pencil: Description
This Windows Service is programmed for personal needs, which communicates with the computer via Telegram bot, to alert the user when the computer is started, shut down, unlocked or locked (In general, if the Windows service is turned off or on, it can be improved to be more accurate if necessary in the future) via telegram bot
![Screenshot 2025-02-22 075116](https://github.com/user-attachments/assets/a198d298-c076-4f30-b03c-356fff31d5b1)
![Screenshot 2025-02-22 075146](https://github.com/user-attachments/assets/4bcd3ce4-9994-4a53-ba55-2431bcf9ea0e)
# :gear: Usage
This service can perform many tasks through certain commands, for example sending or get a file (video images, etc.).
![Screenshot 2025-02-07 181035](https://github.com/user-attachments/assets/1f74d173-57aa-4c8b-9bcf-ff27639943dd)
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
![Screenshot 2025-02-07 181428](https://github.com/user-attachments/assets/a06a5ddb-9536-4c13-b880-0ae54dfcf116)
With script you can also simply start, stop or delete
![Screenshot 2025-02-07 191436](https://github.com/user-attachments/assets/85397602-4959-48d0-aa20-c8bb537f7a4b)
# :hammer_and_wrench: Task Scheduler
The service create a task in "Task Scheduler" which ensures that the service at Windows startup, 
and every hour it is checked that the service is running
# :scroll: Disclaimer
This Windows service is for personal use only, not for commercial use, and we are not responsible for any unethical or wrong use of this Windows service.
This Windows Service is provided "as is", without warranty of any kind, express or implied. The authors and contributors are not responsible for any issues, damages, or losses resulting from the use of this code.
By using this Windows Service, you agree that you do so at your own risk. Always review and test the code before deploying it in production.



