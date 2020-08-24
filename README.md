# uploadService
==============================

Upload Service (Nancy Project)

## IMPORTANT

**You need visual studio to run this program.**
*.Net 4.5 Framework and above require*

## Installation

```open visual studio
install dependencies and build
```

## How to use

HTTP server listening on port `8000`.

## Routes
GET ("/") : All image
POST ("/image/") : Upload image or zip file
GET ("/image/:id") : Get image by ID
GET ("/image/thumb/:id") : Get Image Thumb by ID
GET ("/image/thumb/:id/:type") : Get Image Thumb by ID and type
