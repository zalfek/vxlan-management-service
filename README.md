# vxlan-management-service
VXLAN management service for Technische Hochschule Ulm

## Prerequisite

### Register the service app (Web API)

1. Navigate to the [Azure portal](https://portal.azure.com) and select the **Azure AD** service.
1. Select the **App Registrations** blade on the left, then select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `vxlan-managament-webapi`.
   - Under **Supported account types**, select **Accounts in this organizational directory only**.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**.
1. Select **Save** to save your changes.
1. In the app's registration screen, select the **Expose an API** . To declare an resource URI, follow the following steps:
   - Select `Set` next to the **Application ID URI** to generate a URI that is unique for this app.
   - Accept the proposed Application ID URI (`api://{clientId}`) by selecting **Save**.
1. To publish a scope, follow the following steps:
   - Select **Add a scope** button open the **Add a scope** screen and Enter the values as indicated below:
        - For **Scope name**, use `access_as_user`.
        - Select **Admins and users** options for **Who can consent?**.
        - For **Admin consent display name** type `Access vxlan-managament-webapi`.
        - For **Admin consent description** type `Allows the app to access vxlan-managament-webapi as the signed-in user.`
        - For **User consent display name** type `Access vxlan-managament-webapi`.
        - For **User consent description** type `Allow the application to access vxlan-managament-webapi on your behalf.`
        - Keep **State** as **Enabled**.
        - Select the **Add scope** button on the bottom to save this scope.


### Register the management client app (Webapp)

1. Navigate to the [Azure portal](https://portal.azure.com) and select the **Azure AD** service.
1. Select the **App Registrations** blade on the left, then select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `vxlan-managament-webapp`.
   - Under **Supported account types**, select **Accounts in this organizational directory only**.
   - In the **Redirect URI (optional)** section, select **Web** in the combo-box and enter the following redirect URI: `https://<hosntame or ip>/`.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**.
1. In the app's registration screen, select **Authentication** in the menu.
   - If you don't have a platform added, select **Add a platform** and select the **Web** option.
   - In the **Redirect URIs** section, enter the following redirect URIs.
      - `https://<hosntame or ip>/signin-oidc`
   - In the **Front-channel logout URL** section, set it to `https://localhost:44365/signout-oidc`.
1. Select **Save** to save your changes.
1. In the app's registration screen, select the **Certificates & secrets**
1. In the **Client secrets** section, select **New client secret**:
   - Type a key description (for instance `app secret`),
   - Select one of the available key durations (**In 1 year**, **In 2 years**, or **Never Expires**).
   - The generated key value will be displayed when you select the **Add** button. Note the generated value.
1. In the app's registration screen, select the **API permissions**.
   - Select the **Add a permission** button and then,
   - Ensure that the **My APIs** tab is selected.
   - In the list of APIs, select the API `vxlan-managament-webapi`.
   - In the **Delegated permissions** section, select the **Access 'vxlan-managament-webapi'** in the list. Use the search box if necessary.
   - Select the **Add permissions** button at the bottom.


### Register the connection client app (Linux service)

1. Navigate to the [Azure portal](https://portal.azure.com) and select the **Azure AD** service.
1. Select the **App Registrations** blade on the left, then select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `vxlan-connection-client`.
   - Under **Supported account types**, select **Accounts in this organizational directory only**.
   - In the **Redirect URI (optional)** section, select **Web** in the combo-box and enter the redirect URI which is the static ip of the linux box.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**.
1. In the app's registration screen, select **Authentication** in the menu.
   - If you don't have a platform added, select **Add a platform** and select the **Web** option.
   - In the **Redirect URIs** section, enter the following redirect URIs.
      - `https://<linux box static ip>/signin-oidc`
   - In the **Front-channel logout URL** section, set it to `https://<linux box static ip>/signout-oidc`.
1. Select **Save** to save your changes.
1. In the app's registration screen, select the **Certificates & secrets**
1. In the **Client secrets** section, select **New client secret**:
   - Type a key description (for instance `app secret`),
   - Select one of the available key durations (**In 1 year**, **In 2 years**, or **Never Expires**).
   - The generated key value will be displayed when you select the **Add** button. Note the generated value.
1. In the app's registration screen, select the **API permissions**.
   - Select the **Add a permission** button and then,
   - Ensure that the **My APIs** tab is selected.
   - In the list of APIs, select the API `vxlan-managament-webapi`.
   - In the **Delegated permissions** section, select the **Access 'vxlan-managament-webapi'** in the list. Use the search box if necessary.
   - Select the **Add permissions** button at the bottom.


### Set up of Open Virtual Switch

   1. Install Ip tables and set a rule to DROP all connections except the ones from `vxlan-managament-webapi`.
   2. Install the following tools(example commands for ubuntu2004):
      - `sudo apt -y update`
      - `sudo apt install net-tools`
      - `sudo apt install iftop`
      - `sudo apt-get -y install bpfcc-tools linux-headers-$(uname -r)`
      - `sudo apt -y install python3-pip`
      - `pip3 install pyroute2`
      - `sudo apt -y install openvswitch-switch`
   3. Copy `network_filter.c` and `setup_filter.py` to host machine and execute python script which will compile and append the filter functions to kernel.
   4. Create ssh key(public and private) for switch access.
   5. Go to Overlay Management Client and provide the required info(management, private, public ip and key) and upload  ssh key to open acccess to Overlay Management service.
   6. Certificate should be valid to access the device under root user(or any other user provided in the Management service config).


### Notes

   1. Open Virtual Switch needs 3 addresses to be specified during registration process:
      - Management Ip is the address that will be used by API for management of the Swtich.
      - Private ip address denotes the entry point for the deployed target devices.
      - Public ip denotes the entry point for the client machines..
      When registering Switch or target device in the network, one can also provide hostnames instead of Management Ip, Private ip or Public ip.
      Also 2 or even all 3 ip addresses can be the same meaning that 1 network interface will be used wor everything(management, target devices and clients). 


### Set up of Overlay Connection client(linux service)

   1. install the dotnet framework:
      - `sudo wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb`
      - `sudo dpkg -i packages-microsoft-prod.deb`
      - `sudo rm packages-microsoft-prod.deb`
      - `sudo apt-get install -y apt-transport-https`
      - `sudo apt-get -y update`
      - `sudo apt-get install -y dotnet-sdk-5.0`
   2. Copy the `OverlayConnectionClient.service` file to `/etc/systemd/system/OverlayConnectionClient.service`
   3. Change the ownership and permissions:
      - `sudo chown vagrant <path to build artifacts>/OverlayConnectionClient`
      - `sudo chmod 777 /home/vagrant/srv/OverlayConnectionClient/OverlayConnectionClient`
   4. Reload daemon, configure the service to load at startup and install dev certificates:
      - `sudo systemctl daemon-reload`
      - `sudo systemctl start OverlayConnectionClient`
      - `sudo systemctl enable OverlayConnectionClient`
      - `dotnet dev-certs https`  Only for development purposes. In case of deployment should be used valid certificates.

### Notes

   1. When the linux box connected to the host machine it is accessable via `https://<ip of a linux box>`
   2. It is recommended to have the same ip on all linux boxes.


### Deployment of new devices to the network(non client)
   
   1. Target device is in the same network as Overlay management service or is accessable from that network.
   2. Create ssh key(public and private) for switch access.
   3. Certificate should be valid to access the device under root user(or any other user provided in the Management service config).
   4. Go to Overlay Management Client and provide the required info(management, private, public ip and key) and upload ssh key to open access to Overlay Management service
