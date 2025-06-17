<p align="center">
    <img src="./.github/logo-ordermaker.png" height="100%">
</p>

<p align="center">
  <img alt="GitHub" src="https://img.shields.io/badge/licence-MIT-green">
  <img alt="GitHub" src="https://img.shields.io/badge/platform-.Net%208.0%20%7C%20Windows%20%7C%20Linux-blue">  
  <img alt="GitHub" src="https://img.shields.io/badge/database-MySql%208.00-blue">  
</p>

<hr>

# MTD Key OrderMaker

**MTD Key OrderMaker** — free, open-source platform for designing workflows, managing permissions, and automating approval processes — all without needing extensive coding skills.

Built with enterprise use in mind, it offers fine-grained control over authorization, approval chains, data structures, and much more — helping organizations move from disorganized routines and spreadsheets toward a structured, adaptable, and collaborative process management platform.

---

## 🔹 From chaos to order — your path to freedom

- Are you **stuck in a mess of spreadsheets, Excel files, and disconnected data**?
- **OrderMaker is your way out.**
- Turn chaos into order by designing your own workflows and forms.
- Collect and structure your data, collaborate as a team, and export to Excel for further processing or backup — all while retaining fine-grained control and a clear view of your business processes.

---

## 🔹 Features

- Visual process designer with a **drag-and-drop UI**
- Flexible **role- and group-based authorization** for data and operations
- Approval chains with notifications and task assignment
- Support for custom data fields (list, files, checkboxes, dates, rich text editors, links, etc)
- Multi-application architecture (each with its own data, forms, and workflows)
- Containerized (Docker Compose) for easy enterprise-wide deployment
- Support for retrieving and processing emails directly from a mailbox (IMAP/POP) — create tasks, cases, or collect documents from incoming messages

---

## 🔹 Use Cases

- Small and Medium Businesses — streamline routines and collaborate more effectively
- Large Enterprises — collect data from remote branches and generate reports
- Approval Processes — implement multi-step workflows with notifications, reviews, and authorization
- Custom Applications — create tailored business apps without extensive coding
- Collaborative Knowledge-Based Systems — enable team-wide access with fine-grained permissions
- **For Freelancers and Agencies** — quickly deploy their own platform to serve multiple clients under their own branding, reducing service delivery costs and strengthening their business base

---

## 🔹 User Management

- Implement row-level security with group-specific permissions
- Provide fine-grained authorization controls over documents’ visibility and operations
- Manage users, roles, and groups efficiently within the platform

---

## 🔹 Installation and Deployment

- Containerized with **Docker Compose** for easy enterprise-wide delivery
- Create your `appsettings.json` before starting the application
- [Read the Wiki](wiki link) for more details

---

## 🔹 Video

[🎥 Demo of the request management system](https://www.youtube.com/watch?v=d1oIlVedyVw)

---

## 🔹 Get Started Now

Start using **MTD Key OrderMaker** today:

1. **Installing Docker**  
   Make sure [Docker](https://www.docker.com/) is installed on your machine.

2. **Start the application with Docker Compose**  
   Open your terminal or command console, navigate to the `/source` directory (next to `MtdKey.OrderMaker.sln`), and run:

   ```bash
   docker compose up --build

3. **Access the application**
  Once the containers are up and running, you can view the application in your browser at:
   http://localhost:8080

4. **Administrator Credentials**
You can find the administrator’s login and password in appsettings.Docker.json.

---

## 🔹 License

Copyright (c) presented by [Oleg Bruev](https://github.com/olegbruev/).  
MTD Key OrderMaker is free and open-source software licensed under the MIT License.




