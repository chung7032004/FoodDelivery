import { FaFacebookF, FaInstagram, FaTwitter, FaYoutube, FaTiktok } from 'react-icons/fa';
import { 
  MdDeliveryDining, MdRestaurantMenu, MdLocalOffer, MdStar, 
  MdPayment, MdSecurity, MdSupportAgent
} from 'react-icons/md';
import { HiOutlineDocumentText } from 'react-icons/hi';
import { 
  HiOutlineQuestionMarkCircle, HiOutlinePhone, HiOutlineCash, 
  HiOutlineMail, HiOutlineLocationMarker 
} from 'react-icons/hi';
import { SiVisa, SiMastercard, SiJcb } from 'react-icons/si';
import { IconType } from 'react-icons';

type LinkItem = {
  name: string;
  icon?: IconType;
  href?: string;
};

// Cập nhật FOOTER_LINKS với nhiều nội dung hơn
const FOOTER_LINKS: Record<string, LinkItem[]> = {
  'Khám phá': [
    { name: 'Nhà hàng gần tôi', icon: MdRestaurantMenu },
    { name: 'Món mới mỗi ngày', icon: MdDeliveryDining },
    { name: 'Ưu đãi đặc biệt', icon: MdLocalOffer },
    { name: 'Đánh giá & Xếp hạng', icon: MdStar },
    { name: 'Món ăn phổ biến', icon: MdStar },
  ],
  'Hỗ trợ khách hàng': [
    { name: 'Trung tâm trợ giúp', icon: HiOutlineQuestionMarkCircle },
    { name: 'Liên hệ CSKH', icon: HiOutlinePhone },
    { name: 'Chính sách hoàn tiền', icon: HiOutlineCash },
    { name: 'Hướng dẫn đặt hàng', icon: MdSupportAgent },
    { name: 'Câu hỏi thường gặp', icon: HiOutlineQuestionMarkCircle },
  ],
  'Về EatRush': [
    { name: 'Về chúng tôi' },
    { name: 'Tuyển dụng' },
    { name: 'Blog ẩm thực', icon: HiOutlineDocumentText},
    { name: 'Đối tác nhà hàng' },
    { name: 'Chương trình đại lý' },
  ],
  'Chính sách': [
    { name: 'Điều khoản sử dụng' },
    { name: 'Chính sách bảo mật' },
    { name: 'Chính sách giao hàng' },
    { name: 'Bảo vệ người tiêu dùng', icon: MdSecurity },
  ],
};

const PAYMENT_METHODS = [
  { icon: SiVisa, label: 'Visa' },
  { icon: SiMastercard, label: 'Mastercard' },
  { icon: MdPayment, label: 'MoMo' },
  { icon: SiJcb, label: 'JCB' },
];

const CONTACT_INFO = [
  { icon: HiOutlinePhone, text: '1900 1234', href: 'tel:19001234' },
  { icon: HiOutlineMail, text: 'support@eatrush.vn', href: 'mailto:support@eatrush.vn' },
  { icon: HiOutlineLocationMarker, text: 'TP. Hồ Chí Minh, Việt Nam', href: '#' },
];

export function Footer() {
  return (
    <footer className="bg-gray-900 text-gray-400 pt-12 pb-8 mt-16">
      <div className="max-w-7xl mx-auto px-8">
        {/* Main Footer Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-6 gap-8 mb-10">
          
          {/* Brand Column - Chiếm 2 cột trên màn hình lớn */}
          <div className="lg:col-span-2">
            <div className="font-display text-2xl font-extrabold mb-4">
              <span className="text-brand">Eat</span>
              <span className="text-white">Rush</span>
            </div>
            <p className="text-sm leading-relaxed text-gray-500 mb-4 max-w-md">
              Giao đồ ăn nhanh & ngon tận cửa. Hơn 500 nhà hàng đối tác toàn quốc. 
              Đặt ngay để nhận ưu đãi hấp dẫn!
            </p>
            
            {/* Download App Badges */}
            <div className="flex gap-3 mb-6">
              <img 
                src="https://tools.applemediaservices.com/api/badges/download-on-the-app-store/black/en-us?size=250x83" 
                alt="Download on App Store"
                className="h-10 w-auto"
              />
              <img 
                src="https://upload.wikimedia.org/wikipedia/commons/7/78/Google_Play_Store_badge_EN.svg" 
                alt="Google Play" 
                className="h-10 w-auto"
              />
            </div>
          </div>

          {/* Link Columns */}
          {Object.entries(FOOTER_LINKS).map(([heading, links]) => (
            <div key={heading}>
              <h4 className="text-white text-sm font-semibold mb-4 uppercase tracking-wide">
                {heading}
              </h4>
              <ul className="space-y-2">
                {links.map((link) => (
                  <li key={link.name}>
                    <a
                      href={link.href || '#'}
                      className="text-sm text-gray-500 hover:text-white transition-colors flex items-center gap-2 group"
                    >
                      {link.icon && (
                        <link.icon 
                          size={14} 
                          className="text-gray-600 group-hover:text-brand transition-colors" 
                        />
                      )}
                      <span>{link.name}</span>
                    </a>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>

        {/* Middle Section - Contact & Social */}
        <div className="border-t border-gray-800 pt-8 pb-8">
          <div className="flex flex-col md:flex-row justify-between items-center gap-6">
            {/* Contact Info */}
            <div className="flex flex-wrap gap-6 justify-center">
              {CONTACT_INFO.map(({ icon: Icon, text, href }) => (
                <a
                  key={text}
                  href={href}
                  className="flex items-center gap-2 text-sm text-gray-500 hover:text-white transition-colors"
                >
                  <Icon size={16} className="text-gray-600" />
                  <span>{text}</span>
                </a>
              ))}
            </div>

            {/* Social Icons - CÁCH 3: Màu chỉ hiện khi hover */}
            <div className="flex gap-6">
              <a 
                href="#" 
                className="text-gray-500 hover:text-[#1877F2] transition-all hover:scale-110 duration-200"
                target="_blank"
                rel="noopener noreferrer"
                aria-label="Facebook"
              >
                <FaFacebookF size={20} />
              </a>
              <a 
                href="#" 
                className="text-gray-500 hover:text-[#E4405F] transition-all hover:scale-110 duration-200"
                target="_blank"
                rel="noopener noreferrer"
                aria-label="Instagram"
              >
                <FaInstagram size={20} />
              </a>
              <a 
                href="#" 
                className="text-gray-500 hover:text-[#1DA1F2] transition-all hover:scale-110 duration-200"
                target="_blank"
                rel="noopener noreferrer"
                aria-label="Twitter"
              >
                <FaTwitter size={20} />
              </a>
              <a 
                href="#" 
                className="text-gray-500 hover:text-[#FF0000] transition-all hover:scale-110 duration-200"
                target="_blank"
                rel="noopener noreferrer"
                aria-label="Youtube"
              >
                <FaYoutube size={20} />
              </a>
              <a 
                href="#" 
                className="text-gray-500 hover:text-[#000000] transition-all hover:scale-110 duration-200"
                target="_blank"
                rel="noopener noreferrer"
                aria-label="TikTok"
              >
                <FaTiktok size={20} />
              </a>
            </div>
          </div>
        </div>

        {/* Payment Methods Section */}
        <div className="border-t border-gray-800 pt-6 pb-6">
          <div className="flex flex-col md:flex-row justify-between items-center gap-4">
            <p className="text-xs text-gray-600">Đối tác thanh toán:</p>
            <div className="flex gap-4">
              {PAYMENT_METHODS.map(({ icon: Icon, label }) => (
                <Icon key={label} size={28} className="text-gray-500 hover:text-white transition-colors" />
              ))}
            </div>
          </div>
        </div>

        {/* Copyright */}
        <div className="border-t border-gray-800 pt-6 text-xs text-gray-600 text-center">
          <p>© 2025 EatRush. Được làm với ❤️ tại Việt Nam.</p>
          <p className="mt-1 text-gray-700">Hotline hỗ trợ: 1900 1234 (8:00 - 22:00 hàng ngày)</p>
        </div>
      </div>
    </footer>
  );
}