import { useEffect, useState } from "react";
import { CircleMarker, MapContainer, TileLayer, Tooltip, useMap, useMapEvents } from "react-leaflet";
import { LocateFixed } from "lucide-react";
import Input from "@/components/ui/Input";
import type { LatLngExpression } from "leaflet";
import "leaflet/dist/leaflet.css";

interface Coordinates {
  lat: number;
  lng: number;
}

interface PhotonFeature {
  geometry: { coordinates: [number, number] };
  properties: Record<string, string | undefined>;
}

interface NominatimFeature {
  lat: string;
  lon: string;
  display_name: string;
}

let nominatimQueue = Promise.resolve();
let lastNominatimRequestAt = 0;

const MAP_CONTAINER_STYLE = { width: "100%", height: "320px" };
const DEFAULT_MAP_CENTER: LatLngExpression = [10.7769, 106.7009];

export function AddressMapPicker({
  address,
  position,
  onAddressInput,
  onAddressSelected,
  onPositionSelected,
}: {
  address: string;
  position: Coordinates | null;
  onAddressInput: (address: string) => void;
  onAddressSelected: (address: string, coordinates: Coordinates) => void;
  onPositionSelected: (coordinates: Coordinates) => void;
}) {
  const [searchText, setSearchText] = useState(address);
  const [results, setResults] = useState<PhotonFeature[]>([]);
  const [searchError, setSearchError] = useState("");
  const [isLocating, setIsLocating] = useState(false);

  useEffect(() => setSearchText(address), [address]);

  useEffect(() => {
    const query = searchText.trim();
    if (query.length < 3 || (position && query === address)) {
      setResults([]);
      setSearchError("");
      return;
    }

    const controller = new AbortController();
    const timeout = window.setTimeout(() => {
      void searchPhoton(query, controller.signal)
        .then(setResults)
        .catch((error: unknown) => {
          if (!controller.signal.aborted) {
            setResults([]);
            setSearchError(error instanceof Error ? error.message : "Không thể tìm địa chỉ.");
          }
        });
    }, 350);

    return () => {
      window.clearTimeout(timeout);
      controller.abort();
    };
  }, [address, position, searchText]);

  async function selectCoordinates(coordinates: Coordinates) {
    onPositionSelected(coordinates);
    try {
      const selectedAddress = await reverseGeocodeAddress(coordinates);
      if (selectedAddress) {
        setSearchText(selectedAddress);
        onAddressSelected(selectedAddress, coordinates);
      } else {
        setSearchError("Không tìm thấy địa chỉ tại vị trí đã chọn.");
      }
    } catch {
      setSearchError("Không lấy được tên địa chỉ tại vị trí này.");
    }
  }

  function requestCurrentLocation() {
    if (!navigator.geolocation) {
      setSearchError("Trình duyệt không hỗ trợ lấy vị trí hiện tại.");
      return;
    }
    setIsLocating(true);
    setSearchError("");
    navigator.geolocation.getCurrentPosition(
      ({ coords }) => {
        setIsLocating(false);
        void selectCoordinates({ lat: coords.latitude, lng: coords.longitude });
      },
      () => {
        setSearchError("Không lấy được vị trí. Hãy cho phép truy cập vị trí hoặc chọn vị trí trên bản đồ.");
        setIsLocating(false);
      },
      { enableHighAccuracy: true, timeout: 10000 },
    );
  }

  return (
    <div className="space-y-3">
      <div className="relative">
        <Input
          required
          maxLength={300}
          value={searchText}
          onChange={(event) => {
            setSearchText(event.target.value);
            setResults([]);
            onAddressInput(event.target.value);
          }}
          placeholder="Tìm tên đường, phường, tòa nhà..."
          autoComplete="off"
          fullWidth
        />
        {results.length > 0 && (
          <ul className="absolute inset-x-0 top-full z-[1000] mt-1 max-h-56 overflow-auto rounded-lg border border-slate-200 bg-white shadow-lg">
            {results.map((result, index) => (
              <li key={`${result.geometry.coordinates.join(",")}-${index}`}>
                <button
                  type="button"
                  onClick={() => {
                    const [lng, lat] = result.geometry.coordinates;
                    const coordinates = { lat, lng };
                    const selectedAddress = formatPhotonAddress(result.properties);
                    setSearchText(selectedAddress);
                    setResults([]);
                    setSearchError("");
                    onAddressSelected(selectedAddress, coordinates);
                  }}
                  className="w-full px-3 py-2.5 text-left text-sm text-slate-700 hover:bg-orange-50"
                >
                  {formatPhotonAddress(result.properties)}
                </button>
              </li>
            ))}
          </ul>
        )}
      </div>

      {searchError && <p role="status" className="text-xs text-amber-700">{searchError}</p>}
      <div className="overflow-hidden rounded-lg border border-slate-200">
        <MapContainer center={DEFAULT_MAP_CENTER} zoom={12} scrollWheelZoom style={MAP_CONTAINER_STYLE}>
          <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            url="https://tile.openstreetmap.de/{z}/{x}/{y}.png"
          />
          <MapSelection position={position} onPositionSelected={selectCoordinates} />
          {position && (
            <CircleMarker
              center={[position.lat, position.lng]}
              radius={9}
              pathOptions={{ color: "#fff", weight: 3, fillColor: "#fb6900", fillOpacity: 1 }}
            />
          )}
        </MapContainer>
      </div>
      <button type="button" onClick={requestCurrentLocation} disabled={isLocating} className="inline-flex items-center gap-1.5 text-sm font-medium text-brand hover:underline disabled:opacity-50">
        <LocateFixed size={15} /> {isLocating ? "Đang lấy vị trí..." : "Dùng vị trí hiện tại"}
      </button>
      <p className="text-xs text-muted">Chọn gợi ý tìm kiếm hoặc bấm vào bản đồ để chọn vị trí giao.</p>
    </div>
  );
}

export interface SavedAddressLocation {
  id: string;
  label: string | null;
  latitude: number;
  longitude: number;
  isDefault: boolean;
}

export function AddressOverviewMap({ addresses }: { addresses: SavedAddressLocation[] }) {
  const locations = addresses.filter((address) =>
    Number.isFinite(address.latitude) &&
    Number.isFinite(address.longitude) &&
    !(address.latitude === 0 && address.longitude === 0),
  );

  return (
    <div className="overflow-hidden rounded-lg border border-slate-200">
      <MapContainer center={DEFAULT_MAP_CENTER} zoom={6} scrollWheelZoom style={MAP_CONTAINER_STYLE}>
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://tile.openstreetmap.de/{z}/{x}/{y}.png"
        />
        <FitAddressLocations locations={locations} />
        {locations.map((address) => (
          <CircleMarker
            key={address.id}
            center={[address.latitude, address.longitude]}
            radius={address.isDefault ? 10 : 8}
            pathOptions={{
              color: "#fff",
              weight: 3,
              fillColor: address.isDefault ? "#fb6900" : "#17845d",
              fillOpacity: 1,
            }}
          >
            <Tooltip>{address.label || "Địa chỉ giao hàng"}</Tooltip>
          </CircleMarker>
        ))}
      </MapContainer>
    </div>
  );
}

function FitAddressLocations({ locations }: { locations: SavedAddressLocation[] }) {
  const map = useMap();
  const locationKey = locations.map(({ id, latitude, longitude }) => `${id}:${latitude}:${longitude}`).join("|");

  useEffect(() => {
    if (locations.length === 1) {
      map.setView([locations[0].latitude, locations[0].longitude], 14);
    } else if (locations.length > 1) {
      map.fitBounds(locations.map(({ latitude, longitude }) => [latitude, longitude]), { padding: [32, 32], maxZoom: 14 });
    } else {
      map.setView(DEFAULT_MAP_CENTER, 6);
    }
  }, [locationKey, map]);

  return null;
}

async function searchPhoton(query: string, signal: AbortSignal): Promise<PhotonFeature[]> {
  const params = new URLSearchParams({ q: query, limit: "5" });
  let photonFailure = "không phản hồi";
  try {
    const response = await fetch(`https://photon.komoot.io/api/?${params}`, { signal });
    if (!response.ok) photonFailure = `HTTP ${response.status}`;
    else {
      const data = await response.json() as { features?: PhotonFeature[] };
      if (data.features?.length) return data.features;
    }
  } catch (error) {
    if (signal.aborted) throw error;
    photonFailure = "lỗi kết nối";
  }

  try {
    const nominatimParams = new URLSearchParams({
      q: query,
      format: "jsonv2",
      addressdetails: "1",
      limit: "5",
      countrycodes: "vn",
    });
    const response = await fetchNominatim(`https://nominatim.openstreetmap.org/search?${nominatimParams}`, signal);
    if (!response.ok) throw new Error(`HTTP ${response.status}`);
    const data = await response.json() as NominatimFeature[];
    return data.map((feature) => ({
      geometry: { coordinates: [Number(feature.lon), Number(feature.lat)] },
      properties: { name: feature.display_name },
    }));
  } catch (error) {
    if (signal.aborted) throw error;
    const nominatimFailure = error instanceof Error ? error.message : "lỗi kết nối";
    throw new Error(`Photon (${photonFailure}) và Nominatim (${nominatimFailure}) đều không khả dụng.`);
  }
}

async function reverseGeocodeAddress(coordinates: Coordinates): Promise<string | null> {
  const params = new URLSearchParams({ lat: String(coordinates.lat), lon: String(coordinates.lng) });
  try {
    const response = await fetch(`https://photon.komoot.io/reverse?${params}`);
    if (response.ok) {
      const data = await response.json() as { features?: PhotonFeature[] };
      const feature = data.features?.[0];
      if (feature) return formatPhotonAddress(feature.properties);
    }
  } catch {
    // Try the secondary free geocoder below.
  }

  const nominatimParams = new URLSearchParams({
    lat: String(coordinates.lat),
    lon: String(coordinates.lng),
    format: "jsonv2",
    zoom: "18",
  });
  const response = await fetchNominatim(`https://nominatim.openstreetmap.org/reverse?${nominatimParams}`);
  if (!response.ok) throw new Error(`Không thể tra địa chỉ (HTTP ${response.status}).`);
  const result = await response.json() as { display_name?: string };
  return result.display_name ?? null;
}

function fetchNominatim(url: string, signal?: AbortSignal): Promise<Response> {
  const request = nominatimQueue.then(async () => {
    if (signal?.aborted) throw signal.reason;
    const waitMs = Math.max(0, 1100 - (Date.now() - lastNominatimRequestAt));
    if (waitMs > 0) {
      await new Promise<void>((resolve, reject) => {
        const timeout = window.setTimeout(resolve, waitMs);
        signal?.addEventListener("abort", () => {
          window.clearTimeout(timeout);
          reject(signal.reason);
        }, { once: true });
      });
    }
    if (signal?.aborted) throw signal.reason;
    lastNominatimRequestAt = Date.now();
    return fetch(url, { signal });
  });
  nominatimQueue = request.then(() => undefined, () => undefined);
  return request;
}

function formatPhotonAddress(properties: PhotonFeature["properties"]): string {
  const streetAddress = [properties.housenumber, properties.street].filter(Boolean).join(" ");
  const parts = [
    properties.name && properties.name !== properties.street ? properties.name : null,
    streetAddress || properties.street,
    properties.district,
    properties.city || properties.locality,
    properties.state,
    properties.country,
  ].filter((part): part is string => Boolean(part));
  return [...new Set(parts)].join(", ");
}

function MapSelection({
  position,
  onPositionSelected,
}: {
  position: Coordinates | null;
  onPositionSelected: (coordinates: Coordinates) => void;
}) {
  const map = useMap();
  useEffect(() => {
    if (position) map.flyTo([position.lat, position.lng], 16, { duration: 0.5 });
  }, [map, position?.lat, position?.lng]);
  useMapEvents({
    click: ({ latlng }) => void onPositionSelected({ lat: latlng.lat, lng: latlng.lng }),
  });
  return null;
}
