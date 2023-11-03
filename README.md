# ASP.NETCoreEnterpriseApplications
<details open> 
  <summary> 
    
  ## ASP.NET Core Enterprise Applications - DesenvolvedorIO
  
  </summary>
  
This GitHub repository contains a Microservices Solution built on .NET Core 6, consisting of 7 APIs that collectively form a robust and scalable architecture for various business operations. The solution leverages Microservices architecture, Repository Pattern, JWT Authentication, and AMQP-based integration services for seamless communication between the APIs. Additionally, it includes an Identity and JWT generation Authentication API and a Back-For-Front API, along with a client application following the MVC pattern.

</details>

<details open> 
  <summary>
    
## 📋 Table of Contents
  
  </summary>
  
- [Project Structure](#project-structure)
- [APIs](#apis)
- [Business APIs](#business-apis)
- [Authentication](#authentication-api)
- [Back-For-Front](#back-for-front)
- [Client Application](#client-application)
- [License](#license)
  
</details>

## [Project Structure](#project-structure)
The ASP.NETCoreEnterpriseApplications Solution has a modular structure, with each API residing in its own project. The core components are:

<ul>
  <li><b>NSE.Carrinho.API:</b> Handles shopping cart-related operations.</li>
  <li><b>NSE.Catalogo.API:</b> Manages product catalog and related functionalities.</li>
  <li><b>NSE.Cliente.API:</b> Handles customer information and accounts.</li>
  <li><b>NSE.Pagamento.API:</b> Provides payment processing services.</li>
  <li><b>NSE.Pedido.API:</b> Manages order processing and fulfillment.</li>
  <li><b>Authentication API:</b> Provides user identity management and JWT token generation.</li>
  <li><b>Back-For-Front API:</b> Serves as a middleware layer to simplify communication between the client and the backend microservices.</li>
  <li><b>Client Application:</b> An MVC-based client application to interact with the microservices.</li>
</ul>

##

### [APIs](#apis)

  #### [Business APIs](#business-apis)
  <ul>
    <li><b>NSE.Carrinho.API:</b> Manages shopping cart operations and related data.</li>
    <li><b>NSE.Catalogo.API:</b> Manages product catalog and information.</li>
    <li><b>NSE.Cliente.API:</b> Manages customer information and authentication.</li>
    <li><b>NSE.Pagamento.API:</b> Handles payment processing and transactions.</li>
    <li><b>NSE.Pedido.API:</b> Manages the order processing, status, and fulfillment.</li>
  </ul>
  
  #### [Authentication API](#authentication-api)
  <b>Authentication API:</b> Provides user authentication, identity management, and JWT token generation for secure access to the microservices.

#### [Back-For-Front](#back-for-front)
<b>Back-For-Front API:</b> Acts as an intermediary layer to simplify communication between the client application and the microservices, offering a single entry point for client interactions.

##

### [Client Application](#client-application)
The client application is built using the Model-View-Controller (MVC) pattern. It interacts with the APIs to enable users to browse products, manage their shopping cart, place orders, and perform other relevant operations.

##

## [License](#license)
This Microservices Solution is released under the <a href="./LICENSE" target="_blank">MIT License</a>. 
</br>
Please review the license before using or contributing to this project.
