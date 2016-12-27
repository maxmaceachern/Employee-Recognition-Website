Employee Recognition Project
CS 419 Novelty Workgroup
Ryan Cope, Max MacEachern, and Susan Burns

Introduction
Over the past months, the Novelty workgroup has developed a web portal to facilitate the
generation and delivery of certificates of recognition for employees. This web portal leverages
numerous technologies and tools, and provides a unique and efficient user experience for both
end users and administrative users. The group’s main goal at the outset of this project was to
create an interactive and intuitive service that would be easy for first-time or infrequent users to
master quickly. Through the thoughtful, cooperative evolution of workflows and features, the
group has achieved that goal.

Project Description
The Employee Recognition Project is, first and foremost, a tool for end-users to easily generate
certificates of recognition. From the end-user’s perspective, our web portal offers several
features, including login authentication, user account maintenance, certificate generation, and
certificate management.
Upon visiting the site for the first time, users will be prompted to create a new account with their
name, email, signature image, and other information. After that, the user must confirm their
credentials via email, and then may log in for the first time. If users forget their password at any
point, a password recovery email can be generated to reset it. After logging in, a user may elect
to change their password and other user settings at any point (e.g. changing their signature
image or updating their associated email).
To create a certificate after login, a user navigates to the award creation form and enters the
required data, which includes the award date, recipient’s name and email, the type of award,
and whether the user wants to receive a copy via email. Once the user submits the form, the
certificate entry is stored in that user’s list of certificates, and the PDF certificate is sent to the
recipient’s email address (and the user’s, if elected).
After the creation of the certificate, its details can be viewed. Each certificate is listed with its
corresponding details (award date, type, recipient, etc.). Users may delete certificates from the
portal on this page as well, by clicking the “Delete” link listed with the individual certificate. This
action removes the record of the certificate from the database, but does not retract any emails.
An administrator’s tasks on the website are also quite diverse. Unlike regular end-users,
administrators must be granted access by other administrators. The web portal has a “master”
administrative login that can be used to create the initial, real administrators.
Once the administrator’s credentials are set up, the account maintenance functions like
modifying their associated email are just the same as regular end-users. Because
administrators do not generate certificates, they have a limited number of fields maintained per
account (no signature image, for example).
The main feature of the administrator role is to monitor end-user activity, which is done through
the “Manage Users” page. This page displays a listing of all end-users, including their name,
role, and the date they were created. Administrators can create new users, and can edit or
delete existing users, if needed. The user list can be filtered by various columns, and can be
exported to CSV for more complex manipulation.
Similarly, administrators can take action on other administrative users’ accounts. As mentioned
previously, administrators can only be created by other administrators, and they can also be
edited or deleted. The administrative user list can also be filtered and exported to CSV, if
desired.
Administrators can also create, modify and delete user roles. Roles are assigned to users to
determine what type of access they are allowed (e.g. administrator versus end-user).


Software and System Functions
The Novelty project is hosted on a Windows virtual machine (VM) provisioned through Microsoft
Azure using an ASP.NET MVC 5 framework and SQL Server Express. Upon the initial
deployment of the project, a SQL database is automatically created, along with a handful of
default records for roles and the initial administrator. The ASP.NET MVC framework, with the
help of the ASP.NET Entity Framework, functions side-by-side with the SQL database to create
tables based on the MVC “Models”. These Models, which are written in C#, all contain class
specifications that are used in MVC “Controllers” (also written in C#). The actual web pages
themselves are rendered with MVC’s “Views”, which are .cshtml files that consume the data
presented by the Models. In this way, the SQL database is truly the central point around which
the rest of the MVC framework is oriented.
During the rendering of the HTML pages, Bootstrap’s CSS framework is leveraged to provide a
consistent and professional aesthetic.
The generation of the PDF certificates is accomplished with command-line calls to the pdflatex
compiler from a Controller, leveraging a template certificate .tex document along with the
user-provided form data. To enable the pdflatex call, the MiKTeX distribution is installed on the
hosting VM. The requirement to convert the certificate from LaTeX to PDF (and the subsequent
need for a TeX distribution on our server) is the main reason why a VM is used instead of
Azure’s cloud services or app services for hosting.
Emails sent for certificate generation, user account confirmation, and password recovery are all
done using calls from Controllers along with Gmail’s SMTP services. While this solution is
adequate for our (low-volume) purposes, any significant scaling of the project would require an
alternative SMTP solution.
The development of this project was done in the Visual Studio IDE, while version control and
code sharing was done on GitHub. The creation and testing of the .tex certificate template was
done using TeXworks. Group communication was facilitated via email and Slack.

Technology Inventory
Languages:
● C#
● HTML
● TeX
Libraries & Frameworks:
● MiKTeX
● ASP.NET MVC 5
● ASP.NET Entity Framework
● ASP.NET Identity
● Bootstrap
Tools & Services:
● Gmail SMTP Service
● TeXworks
● Visual Studio IDE
● Azure Virtual Machine
● SQL Server Express
● GitHub
● Slack
