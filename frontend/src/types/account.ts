export interface AccountResponse {
  id: string;
  fullName: string | null;
  email: string | null;
  phone: string | null;
  avatarUrl: string | null;
  roles: string[];
}

export interface UserRoleRestriction {
  restrictionId: string;
  roleName: string;
  reason: string;
  restrictedAt: string;
  appealId: string | null;
  appealReason: string | null;
  appealStatus: 'Pending' | 'Rejected' | 'Approved' | null;
  appealSubmittedAt: string | null;
  appealResponse: string | null;
}

export interface AddressFormState {
  fullAddress: string;
  label: string;
  receiverName: string;
  phoneNumber: string;
  latitude: string;
  longitude: string;
  isDefault: boolean;
}

