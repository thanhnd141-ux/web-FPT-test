import React from "react";
import { useNavigate } from "react-router-dom";
import { setNavigate, useAuthStore } from "../../store";

const AuthPage: React.FC = () => {
  const navigate = useNavigate();
  const [isLogin, setIsLogin] = React.useState(true);
  const { login, register, isLoading } = useAuthStore();

  // Thiết lập navigation function cho store
  React.useEffect(() => {
    setNavigate(navigate);
  }, [navigate]);

  const [formData, setFormData] = React.useState({
    email: "",
    password: "",
    name: "",
    phoneNumber: "",
  });
  const [error, setError] = React.useState("");
  const handleEmailPasswordSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    try {
      if (isLogin) {
        await login(formData.email, formData.password);
      } else {
        await register(
          formData.email,
          formData.password,
          formData.name,
          formData.phoneNumber
        );
      }
    } catch (error) {
      setError(error instanceof Error ? error.message : "Đã xảy ra lỗi");
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };
  const resetForm = () => {
    setFormData({
      email: "",
      password: "",
      name: "",
      phoneNumber: "",
    });
    setError("");
  };

  const toggleLoginRegister = () => {
    setIsLogin(!isLogin);
    resetForm();
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-orange-50 via-pink-50 to-peach-50 py-12 px-4 sm:px-6 lg:px-8 relative overflow-hidden">
      {/* Decorative Background Elements */}
      <div className="absolute top-0 left-0 w-72 h-72 bg-gradient-to-br from-orange-300/30 to-pink-300/30 rounded-full blur-3xl -translate-x-1/2 -translate-y-1/2"></div>
      <div className="absolute bottom-0 right-0 w-96 h-96 bg-gradient-to-br from-pink-300/30 to-peach-300/30 rounded-full blur-3xl translate-x-1/2 translate-y-1/2"></div>

      <div className="max-w-md w-full space-y-8 relative z-10">
        {/* Header */}
        <div className="text-center">
          <div className="mx-auto mb-6 inline-block">
            <div className="w-20 h-20 bg-gradient-to-br from-orange-400 to-pink-500 rounded-3xl flex items-center justify-center shadow-2xl shadow-pink-300/50 transform hover:scale-110 transition-transform">
              <span className="text-5xl">✨</span>
            </div>
          </div>{" "}
          <h2 className="text-4xl font-extrabold bg-gradient-to-r from-orange-500 to-pink-600 bg-clip-text text-transparent mb-3">
            Welcome Back! 🎉
          </h2>
          <p className="text-base text-gray-600 font-medium">
            {isLogin ? "✉️ Đăng nhập với email" : "✉️ Đăng ký ngay"}
          </p>
        </div>

        {/* Forms */}
        <div className="bg-white rounded-3xl shadow-2xl p-8 backdrop-blur-sm border border-white/50">
          <form onSubmit={handleEmailPasswordSubmit} className="space-y-5">
            {!isLogin && (
              <div>
                <label
                  htmlFor="name"
                  className="block text-sm font-bold text-gray-700 mb-2"
                >
                  👤 Họ và tên
                </label>
                <input
                  id="name"
                  name="name"
                  type="text"
                  required={!isLogin}
                  className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-pink-400 focus:border-transparent transition-all"
                  placeholder="Nhập họ và tên của bạn"
                  value={formData.name}
                  onChange={handleChange}
                />
              </div>
            )}
            <div>
              <label
                htmlFor="email"
                className="block text-sm font-bold text-gray-700 mb-2"
              >
                ✉️ Địa chỉ email
              </label>
              <input
                id="email"
                name="email"
                type="email"
                autoComplete="email"
                required
                className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-pink-400 focus:border-transparent transition-all"
                placeholder="email@example.com"
                value={formData.email}
                onChange={handleChange}
              />
            </div>
            <div>
              <label
                htmlFor="password"
                className="block text-sm font-bold text-gray-700 mb-2"
              >
                🔒 Mật khẩu
              </label>
              <input
                id="password"
                name="password"
                type="password"
                autoComplete={isLogin ? "current-password" : "new-password"}
                required
                className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-pink-400 focus:border-transparent transition-all"
                placeholder={isLogin ? "Nhập mật khẩu" : "Tạo mật khẩu mạnh"}
                value={formData.password}
                onChange={handleChange}
              />
            </div>
            {!isLogin && (
              <div>
                <label
                  htmlFor="phoneNumber"
                  className="block text-sm font-bold text-gray-700 mb-2"
                >
                  📱 Số điện thoại (tuỳ chọn)
                </label>
                <input
                  id="phoneNumber"
                  name="phoneNumber"
                  type="tel"
                  className="w-full px-4 py-3 border-2 border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-pink-400 focus:border-transparent transition-all"
                  placeholder="0901234567"
                  value={formData.phoneNumber}
                  onChange={handleChange}
                />
              </div>
            )}
          </form>
          {/* Error Message */}
          {error && (
            <div className="mt-5 p-4 bg-red-50 border-2 border-red-200 text-red-700 rounded-xl text-sm flex items-start gap-3">
              <span className="text-2xl">⚠️</span>
              <span className="flex-1 font-medium">{error}</span>
            </div>
          )}{" "}
          {/* Submit Button */}
          <div className="mt-6">
            <button
              type="submit"
              className="w-full bg-gradient-to-r from-orange-500 to-pink-600 hover:from-orange-600 hover:to-pink-700 text-white font-bold py-4 px-6 rounded-xl transition-all shadow-lg hover:shadow-2xl hover:scale-105 transform disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none"
              disabled={isLoading}
              onClick={handleEmailPasswordSubmit}
            >
              {isLoading ? (
                <div className="flex items-center justify-center gap-3">
                  <svg
                    className="animate-spin h-5 w-5 text-white"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <circle
                      className="opacity-25"
                      cx="12"
                      cy="12"
                      r="10"
                      stroke="currentColor"
                      strokeWidth="4"
                    ></circle>
                    <path
                      className="opacity-75"
                      fill="currentColor"
                      d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                    ></path>
                  </svg>
                  <span>Đang xử lý...</span>
                </div>
              ) : (
                <span>
                  {isLogin ? "🚀 Đăng nhập ngay" : "✨ Tạo tài khoản"}
                </span>
              )}
            </button>
          </div>
          {/* Toggle Login/Register */}
          <div className="mt-6 text-center">
            <button
              type="button"
              className="text-sm font-bold bg-gradient-to-r from-orange-500 to-pink-600 bg-clip-text text-transparent hover:from-orange-600 hover:to-pink-700 transition-all"
              onClick={toggleLoginRegister}
            >
              {isLogin
                ? "✨ Chưa có tài khoản? Tạo tài khoản mới"
                : "👋 Đã có tài khoản? Đăng nhập ngay"}
            </button>
          </div>
        </div>

        {/* Additional Info */}
        <div className="text-center text-sm text-gray-500">
          <p>Bằng cách đăng nhập, bạn đồng ý với</p>
          <p className="mt-1">
            <a
              href="#"
              className="text-pink-500 hover:text-pink-600 font-medium"
            >
              Điều khoản sử dụng
            </a>
            {" & "}
            <a
              href="#"
              className="text-pink-500 hover:text-pink-600 font-medium"
            >
              Chính sách bảo mật
            </a>
          </p>
        </div>
      </div>
    </div>
  );
};
        
export default AuthPage;
