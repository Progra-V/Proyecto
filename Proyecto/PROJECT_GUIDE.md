# HelpDesk Pro

## Project Overview

HelpDesk Pro is a web-based ticket management system developed with ASP.NET Core MVC.

The application allows employees to create support requests while technical staff manage, assign and resolve tickets.

---

# Technology Stack

- ASP.NET Core MVC
- C#
- Entity Framework Core
- Supabase
- Razor Views
- Bootstrap 5
- Bootstrap Icons

---

# Architecture

Controllers

Responsible only for handling requests and returning views.

No business logic should be implemented here.

---

Services

Contain all business logic.

Examples:

- TicketService
- CategoryService
- UserService
- SystemService

---

Models

Represent database tables.

---

ViewModels

Represent information required by a View.

They are NOT database tables.

---

Views

Responsible only for displaying information.

Business logic should never be implemented inside Views.

---

# Project Structure

Proyecto

│

├── Controllers

├── Models

├── Services

├── ViewModels

├── Views

│

├── Dashboard

├── Ticket

├── Category

├── System

├── User

│

└── Shared

        _Layout

        _NavMenu

        _DashboardCards

        _RecentTickets

        _Workspace

---

# UI Guidelines

All modules should follow the same layout.

Cards

↓

Content

↓

Actions

Examples:

Dashboard

Cards

↓

Recent Tickets

↓

Workspace

Category

Cards

↓

Table

↓

Category Form

System

Cards

↓

Table

↓

System Form

---

# Color Palette

Primary

Blue

Success

Green

Warning

Yellow

Danger

Red

Sidebar

Dark Blue

Background

Light Gray

Cards

White

---

# Coding Standards

Controllers

Only coordinate requests.

Services

Business logic.

Views

Presentation only.

Models

Represent database tables.

ViewModels

Represent screens.

---

# Naming Conventions

Controllers

CategoryController

TicketController

DashboardController

Services

CategoryService

TicketService

DashboardService

Models

Category

Ticket

System

Views

Index

Create

Edit

Detail

---

# Development Rules

Always use Partial Views when a component is reused.

Do not duplicate code.

Controllers should remain clean.

Business logic belongs in Services.

Views should remain simple.

---

# Git Workflow

Each developer works in an independent branch.

Merge only after testing.

Review changes before merging.

Never modify another developer's module without communication.

---

# Current Modules

✔ Login

✔ Dashboard

✔ Tickets

🚧 Categories

🚧 Systems

⏳ Users

⏳ Priorities

⏳ Risks

⏳ Status

---

# Future Improvements

Role-based dashboard

Notifications

Reports

Charts

Dark Mode

Email Notifications

Audit Log