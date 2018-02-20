# AdminCenter

dotnet core Admin Manager

### 简介
- 最近准备使用dotnet core 2.0 开发小程序外卖平台和一个后台管理系统，我是使用mac os系统的，所以从visual studio 官方下载了 vs2017 for mac 安装的时候如果勾选了所有，需要在线下载4G内容，我刚开始安装就勾选了所有，结果安装了一天都没安装成功，估计是需要国外(翻墙)下载比较慢，于是就取消了重新安装，第二次安装把app开发的去除了，就剩下.net core开发，整个就1G多，几分钟就安装完了。

- 准备开始开发了，于是新建了个.net core mvc项目，从网上找了个登录模板，就开始开发了，登录必须要验证码，于是把以前的验证码类复制过来，这时傻眼了，完全不能用，于是从网上找到了一个验证码类(引用nuget包ZKWeb.System.Drawing)，验证码是需要服务端校验的，接着使用session,又懵了，之前的asp.net 完全不能用，于是又从google下了解了，.net core 中微软把session升级了([详细了解](http://blog.csdn.net/slowlifes/article/details/71077730))，可以做到共享session,为分布式做好了铺垫,.net core 需要引用nuget包(Microsoft.AspNetCore.Session);

- 谈谈vs2017 for mac的使用感受，前端razor完全没有提示，格式化也没有效果，并且对系统性能要求很高，经常出现卡顿，跟windows完全无法比。写好代码部署的时候需要发布，只能发布到微软的azure上，没有发布到本地的选项，还得用终端自己发布。

- 谈谈部署的情况，首先发布完之后需要本地测试下，用的是mac os系统，你会发现验证码无法显示，本地运行好好的，为什么发布后就不行了？这是因为nuget包ZKWeb.System.Drawing需要各个系统的兼容。

1. dotnet publish -c release
2. 将/bin/Release/netcoreapp2.0/publish 上传到服务器
3. 服务器上安装dotnet 环境

```
sudo yum update
sudo yum install libunwind libicu
sudo yum install dotnet-sdk-2.0.0
```

#### 大叔总结方法

1. ubuntu && debian

```
sudo apt-get install libgdiplus
cd /usr/lib
sudo ln -s libgdiplus.so gdiplus.dll
```

2. centos

```
yum whatprovides libgdiplus && yum install -y epel-release && yum install -y libgdiplus-2.10-9.el7.x86_64 && yum install -y libgdiplus-devel
```

#### 官方方法

1. ubuntu系统

```
apt-get install libgdiplus
cd /usr/lib
ln -s libgdiplus.so gdiplus.dll
```

2. Fedora

```
dnf install libgdiplus
cd /usr/lib64/
ln -s libgdiplus.so.0 gdiplus.dll
```

3. centos系统

```
yum install autoconf automake libtool
yum install freetype-devel fontconfig libXft-devel
yum install libjpeg-turbo-devel libpng-devel giflib-devel libtiff-devel libexif-devel
yum install glib2-devel cairo-devel
yum install git
git clone https://github.com/mono/libgdiplus
cd libgdiplus
yum -y install ftp
./autogen.sh
yum -y install gcc automake autoconf libtool make
yum -y install gcc gcc-c++
make
make install
cd /usr/lib64/
ln -s /usr/local/lib/libgdiplus.so gdiplus.dll
```

4. mac os系统

```
brew install mono-libgdiplus
```

- 由于使用dotnet 运行应运是在shell下运行的，关闭了就停止了，所以需要配置对ASP.NET Core应用的守护

1. 启动项目命令

```
dotnet publish/{项目名称}.dll
```

2. 配置守护Supervisor

- 安装Supervisor

```
yum install python-setuptools

easy_install supervisor
```

- 配置Supervisor

```
mkdir /etc/supervisor

echo_supervisord_conf > /etc/supervisor/supervisord.conf
```

修改supervisord.conf文件，将文件尾部的配置

```
;[include]
;files = relative/directory/*.ini
```

改成

```
[include]
files = conf.d/*.conf
```

- 配置对ASP.NET Core应用的守护

创建一个 {应用名称}.conf文件，内容大致如下

```
[program:{应用名称}]
command=dotnet {应用名称}.dll ; 运行程序的命令
directory=/var/www/publish/{应用名称}/ ; 命令执行的目录
autorestart=true ; 程序意外退出是否自动重启
stderr_logfile=/var/log/{应用名称}.err.log ; 错误日志文件
stdout_logfile=/var/log/{应用名称}.out.log ; 输出日志文件
environment=ASPNETCORE_ENVIRONMENT=Production ; 进程环境变量
user=root ; 进程执行的用户身份
stopsignal=INT
```

将文件拷贝至：“/etc/supervisor/conf.d/{应用名称}.conf”下

运行supervisord，查看是否生效

```
supervisord -c /etc/supervisor/supervisord.conf

ps -ef | grep {应用名称}
```

- 解决切换应用问题

> Error: Another program is already listening on a port that one of our HTTP servers is configured to use. Shut this program down first before starting

```
find / -name supervisor.sock

unlink /name/supervisor.sock
```

- Supervisor 常用命令

```
supervisorctl start all

supervisorctl stop all

supervisorctl status

supervisorctl update

supervisorctl showdown
```

- 配置Supervisor开机启动

新建一个“supervisord.service”文件

```
# dservice for systemd (CentOS 7.0+)
# by ET-CS (https://github.com/ET-CS)
[Unit]
Description=Supervisor daemon

[Service]
Type=forking
ExecStart=/usr/bin/supervisord -c /etc/supervisor/supervisord.conf
ExecStop=/usr/bin/supervisorctl shutdown
ExecReload=/usr/bin/supervisorctl reload
KillMode=process
Restart=on-failure
RestartSec=42s

[Install]
WantedBy=multi-user.target
```

将文件拷贝至：“/usr/lib/systemd/system/supervisord.service”

```
systemctl enable supervisord

#来验证是否为开机启动
systemctl is-enabled supervisord
```

- 端口没有激活

```
iptables -A INPUT -p tcp --dport 5000 -j ACCEPT  
```

- 端口被占用

```
 netstat -lnp|grep 5000

 kill {pid}
```