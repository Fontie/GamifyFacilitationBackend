import http.server
import socketserver
import mimetypes

class BrotliHTTPRequestHandler(http.server.SimpleHTTPRequestHandler):
    def guess_type(self, path):
        if path.endswith(".br"):
            return "application/javascript"
        return super().guess_type(path)

    def end_headers(self):
        if self.path.endswith(".br"):
            self.send_header("Content-Encoding", "br")
        super().end_headers()

PORT = 8000
Handler = BrotliHTTPRequestHandler
httpd = socketserver.TCPServer(("0.0.0.0", PORT), Handler)

print(f"Serving on https://localhost:{PORT}")
httpd.serve_forever()
