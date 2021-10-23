#!/bin/bash
ssh root@maluch.mikr.us -p 10104 'rm -rf /var/scada/front'
dotnet publish -c Release -o linux --runtime linux-x64 --self-contained false
scp -P 10104 -r ./linux root@maluch.mikr.us:/var/scada/front
ssh root@maluch.mikr.us -p 10104 'chown -R www /var/scada/front; /root/restartscada.sh'