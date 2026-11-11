import { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { CheckCircle2, XCircle, ArrowRight } from "lucide-react";
import { api } from "@/services/api";

export function PaymentCallbackPage() {
    const [params] = useSearchParams();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [isSuccess, setIsSuccess] = useState(false);
    const [message, setMessage] = useState("");

    const orderInfo = params.get("vnp_OrderInfo");
    const amount = Number(params.get("vnp_Amount") || 0) / 100;

    useEffect(() => {
        async function verify() {
            try {
                const { data } = await api.get(`/payment/vnpay-verify?${params.toString()}`);
                if (data.success) {
                    setIsSuccess(true);
                    setMessage("Giao dịch thanh toán thành công!");
                } else {
                    setIsSuccess(false);
                    setMessage(data.message || "Thanh toán không thành công.");
                }
            } catch (err: any) {
                setIsSuccess(false);
                setMessage(err.response?.data?.message || "Xác thực thanh toán thất bại.");
            } finally {
                setLoading(false);
            }
        }
        verify();
    }, [params]);

    if (loading) {
        return (
            <div className="flex min-h-[60vh] flex-col items-center justify-center">
                <div className="h-12 w-12 animate-spin rounded-full border-4 border-orange-500 border-t-transparent"></div>
                <p className="mt-4 font-medium text-slate-600">Đang kiểm tra kết quả giao dịch...</p>
            </div>
        );
    }

    return (
        <div className="mx-auto max-w-lg px-4 py-16">
            <div className="rounded-3xl border border-slate-100 bg-white p-8 text-center shadow-xl">
                {isSuccess ? (
                    <>
                        <CheckCircle2 className="mx-auto h-16 w-16 text-emerald-500" />
                        <h1 className="mt-4 text-2xl font-bold text-slate-900">Thanh toán thành công!</h1>
                        <p className="mt-2 text-slate-500">{orderInfo}</p>
                        <p className="mt-4 text-3xl font-extrabold text-[#fb6900]">
                            {new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(amount)}
                        </p>
                    </>
                ) : (
                    <>
                        <XCircle className="mx-auto h-16 w-16 text-red-500" />
                        <h1 className="mt-4 text-2xl font-bold text-slate-900">Thanh toán thất bại</h1>
                        <p className="mt-2 text-slate-500">{message}</p>
                    </>
                )}

                <div className="mt-8 flex flex-col gap-3">
                    <button
                        onClick={() => navigate("/account?tab=orders")}
                        className="flex w-full items-center justify-center gap-2 rounded-xl bg-[#fb6900] py-3.5 font-bold text-white transition hover:bg-[#e05d00]"
                    >
                        Xem đơn hàng của bạn <ArrowRight className="h-4 w-4" />
                    </button>
                    <button
                        onClick={() => navigate("/")}
                        className="w-full rounded-xl border border-slate-200 py-3 font-semibold text-slate-600 hover:bg-slate-50"
                    >
                        Về trang chủ
                    </button>
                </div>
            </div>
        </div>
    );
}