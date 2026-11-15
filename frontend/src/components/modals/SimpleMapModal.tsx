import { useEffect } from "react";
import { CircleMarker, MapContainer, TileLayer, Tooltip, useMap } from "react-leaflet";
import { X } from "lucide-react";
import type { LatLngExpression } from "leaflet";
import type { MapLocation } from "@/types";
import "leaflet/dist/leaflet.css";

export type { MapLocation };

interface SimpleMapModalProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  subtitle?: string;
  locations: MapLocation[];
}

const DEFAULT_MAP_CENTER: LatLngExpression = [10.7769, 106.7009];
const MAP_CONTAINER_STYLE = { width: "100%", height: "350px" };

export function SimpleMapModal({
  isOpen,
  onClose,
  title = "Xem bản đồ",
  subtitle,
  locations,
}: SimpleMapModalProps) {
  if (!isOpen) return null;

  const validLocations = locations.filter(
    (loc) => Number.isFinite(loc.lat) && Number.isFinite(loc.lng) && !(loc.lat === 0 && loc.lng === 0),
  );

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4 backdrop-blur-sm">
      <div className="w-full max-w-xl overflow-hidden rounded-2xl bg-white shadow-2xl animate-in fade-in zoom-in-95 duration-200">
        {/* Modal Header */}
        <div className="flex items-center justify-between border-b border-slate-100 px-6 py-4">
          <div>
            <h3 className="font-display text-lg font-bold text-slate-900">{title}</h3>
            {subtitle && <p className="text-xs text-slate-500 line-clamp-1">{subtitle}</p>}
          </div>
          <button
            onClick={onClose}
            className="rounded-lg p-1.5 text-slate-400 hover:bg-slate-100 hover:text-slate-700 transition"
          >
            <X size={20} />
          </button>
        </div>

        {/* Modal Body (Map Container) */}
        <div className="p-4">
          <div className="overflow-hidden rounded-xl border border-slate-200">
            <MapContainer center={DEFAULT_MAP_CENTER} zoom={12} scrollWheelZoom={true} style={MAP_CONTAINER_STYLE}>
              <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                url="https://tile.openstreetmap.de/{z}/{x}/{y}.png"
              />
              <FitMapBounds locations={validLocations} />
              {validLocations.map((loc, index) => (
                <CircleMarker
                  key={loc.id ?? `${loc.lat}-${loc.lng}-${index}`}
                  center={[loc.lat, loc.lng]}
                  radius={loc.isDefault ? 10 : 8}
                  pathOptions={{
                    color: "#fff",
                    weight: 3,
                    fillColor: loc.isDefault ? "#fb6900" : "#17845d",
                    fillOpacity: 1,
                  }}
                >
                  {loc.label && <Tooltip>{loc.label}</Tooltip>}
                </CircleMarker>
              ))}
            </MapContainer>
          </div>
        </div>

        {/* Modal Footer */}
        <div className="flex items-center justify-end gap-3 border-t border-slate-100 bg-slate-50 px-6 py-3">
          <button
            onClick={onClose}
            className="rounded-lg bg-slate-900 px-4 py-2 text-sm font-medium text-white hover:bg-slate-800 transition"
          >
            Đóng
          </button>
        </div>
      </div>
    </div>
  );
}

function FitMapBounds({ locations }: { locations: MapLocation[] }) {
  const map = useMap();
  const locationKey = locations.map(({ lat, lng }) => `${lat}:${lng}`).join("|");

  useEffect(() => {
    if (locations.length === 1) {
      map.setView([locations[0].lat, locations[0].lng], 15);
    } else if (locations.length > 1) {
      map.fitBounds(
        locations.map(({ lat, lng }) => [lat, lng]),
        { padding: [32, 32], maxZoom: 15 },
      );
    } else {
      map.setView([0, 0], 6);
    }
  }, [locationKey, map]);

  return null;
}