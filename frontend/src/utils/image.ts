const BACKEND_BASE_URL = import.meta.env.VITE_API_URL.replace(/\/api\/?$/, "");
// "https://localhost:7147/api" -> "https://localhost:7147"

export function getImageUrl(path?: string | null): string {
  if (!path) return "/placeholder-food.png";
  if (path.startsWith("http")) return path;
  return `${BACKEND_BASE_URL}${path}`;
}