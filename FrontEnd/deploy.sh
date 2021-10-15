#!/bin/bash
dotnet publish -c Release -o linux --runtime linux-x64 --self-contained false
scp -P 10104 -r ./linux root@maluch.mikr.us:/var/scada/front